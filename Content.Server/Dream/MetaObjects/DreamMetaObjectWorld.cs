﻿using Content.Server.DM;
using OpenDreamShared.Dream;
using Robust.Shared.IoC;
using Robust.Shared.Timing;
using System;

namespace Content.Server.Dream.MetaObjects {
    class DreamMetaObjectWorld : DreamMetaObjectRoot {
        [Dependency] private IDreamManager _dreamManager;
        [Dependency] private IGameTiming _gameTiming;

        private ViewRange _viewRange;

        public DreamMetaObjectWorld() {
            IoCManager.InjectDependencies(this);
        }

        public override void OnObjectCreated(DreamObject dreamObject, DreamProcArguments creationArguments) {
            base.OnObjectCreated(dreamObject, creationArguments);

            _dreamManager.WorldContentsList = dreamObject.GetVariable("contents").GetValueAsDreamList();

            dreamObject.SetVariable("log", new DreamValue(new ConsoleOutputResource()));

            DreamValue fps = dreamObject.ObjectDefinition.Variables["fps"];
            if (fps.Value != null) {
                _gameTiming.TickRate = (byte)fps.GetValueAsInteger();
            }

            DreamValue view = dreamObject.ObjectDefinition.Variables["view"];
            if (view.TryGetValueAsString(out string viewString)) {
                _viewRange = new ViewRange(viewString);
            } else {
                _viewRange = new ViewRange(view.GetValueAsInteger());
            }

            //New() is not called here
        }

        public override void OnVariableSet(DreamObject dreamObject, string variableName, DreamValue variableValue, DreamValue oldVariableValue) {
            base.OnVariableSet(dreamObject, variableName, variableValue, oldVariableValue);

            switch (variableName) {
                case "fps":
                    _gameTiming.TickRate = (byte)variableValue.GetValueAsInteger(); break;
                case "maxz": {
                    int newMaxZ = variableValue.GetValueAsInteger();

                    /*if (newMaxZ < Runtime.Map.Levels.Count) {
                        while (Runtime.Map.Levels.Count > newMaxZ) {
                            Runtime.Map.RemoveLevel();
                        }
                    } else {
                        while (Runtime.Map.Levels.Count < newMaxZ) {
                            Runtime.Map.AddLevel();
                        }
                    }*/

                    break;
                }
            }
        }

        public override DreamValue OnVariableGet(DreamObject dreamObject, string variableName, DreamValue variableValue) {
            switch (variableName) {
                case "tick_lag":
                    return new DreamValue(_gameTiming.TickPeriod.TotalMilliseconds / 100);
                case "fps":
                    return new DreamValue(_gameTiming.TickRate);
                case "timeofday":
                    return new DreamValue((int)DateTime.UtcNow.TimeOfDay.TotalMilliseconds / 100);
                case "time":
                    return new DreamValue(_gameTiming.ServerTime.TotalMilliseconds / 100);
                case "realtime":
                    return new DreamValue((DateTime.Now - new DateTime(2000, 1, 1)).Milliseconds / 100);
                case "tick_usage": {
                    //TODO: This can only go up to 100%, tick_usage should be able to go higher
                    return new DreamValue((float)_gameTiming.TickFraction / ushort.MaxValue);
                }
                //case "maxx":
                //    return new DreamValue(Runtime.Map.Width);
                //case "maxy":
                //    return new DreamValue(Runtime.Map.Height);
                //case "maxz":
                //    return new DreamValue(Runtime.Map.Levels.Count);
                //case "address":
                //    return new(Runtime.Server.Address.ToString());
                //case "port":
                //    return new(Runtime.Server.Port);
                //case "url":
                //    return new("opendream://" + Runtime.Server.Address + ":" + Runtime.Server.Port);
                case "system_type": {
                    //system_type value should match the defines in Defines.dm
                    if (Environment.OSVersion.Platform is PlatformID.Unix or PlatformID.MacOSX or PlatformID.Other) {
                        return new DreamValue(0);
                    }
                    //Windows
                    return new DreamValue(1);
                }
                case "view": {
                    //Number if square & centerable, string representation otherwise
                    return new DreamValue((_viewRange.IsSquare && _viewRange.IsCenterable) ? _viewRange.Width : _viewRange.ToString());
                }
                default:
                    return base.OnVariableGet(dreamObject, variableName, variableValue);
            }
        }

        public override DreamValue OperatorOutput(DreamValue a, DreamValue b) {
            //foreach (DreamConnection connection in Runtime.Server.Connections) {
            //    connection.OutputDreamValue(b);
            //}

            return new DreamValue(0);
        }
    }
}
