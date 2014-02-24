﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exceptionless;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.ExceptionLess
{
    public class ExceptionLessSink : ILogEventSink
    {
        private readonly Func<ErrorBuilder, ErrorBuilder> _additionalOperation;
        private readonly bool _includeProperties;

        public ExceptionLessSink( Func<ErrorBuilder, ErrorBuilder> additionalOperation = null, bool includeProperties= true)
        {
            _additionalOperation = additionalOperation;
            _includeProperties = includeProperties;
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
                throw new ArgumentNullException("logEvent");

            if (logEvent.Exception == null)
                return;

            ErrorBuilder errorBuilder = logEvent.Exception.ToExceptionless();
            if (logEvent.Level == LogEventLevel.Fatal)
            {
                errorBuilder.MarkAsCritical();
            }

            errorBuilder.AddObject(logEvent.RenderMessage(), "Log Message");

            if (_includeProperties && logEvent.Properties != null && logEvent.Properties.Count != 0)
            {
                foreach (var property in logEvent.Properties)
                {
                    errorBuilder.AddObject(property.Value, property.Key);
                }                
            }
            
            if (_additionalOperation != null)
                _additionalOperation(errorBuilder);

            errorBuilder.Submit();
        }
    }
}
