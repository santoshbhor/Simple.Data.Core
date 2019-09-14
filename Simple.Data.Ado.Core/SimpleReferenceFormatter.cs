using System.ComponentModel;

namespace Simple.Data.Ado
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Schema;

    public class SimpleReferenceFormatter
    {
        private readonly IFunctionNameConverter _functionNameConverter;
        private readonly DatabaseSchema _schema;
        private readonly ICommandBuilder _commandBuilder;

        public SimpleReferenceFormatter(DatabaseSchema schema, ICommandBuilder commandBuilder) : this(schema, commandBuilder, null)
        {

        }

        public SimpleReferenceFormatter(DatabaseSchema schema, ICommandBuilder commandBuilder, IFunctionNameConverter functionNameConverter)
        {
            _schema = schema;
            _commandBuilder = commandBuilder;
            _functionNameConverter = functionNameConverter ?? new FunctionNameConverter();
        }

        public string FormatColumnClause(SimpleReference reference)
        {
            return FormatColumnClause(reference, false);
        }

        public string FormatColumnClauseWithoutAlias(SimpleReference reference)
        {
            return FormatColumnClause(reference, true);
        }

        private string FormatColumnClause(SimpleReference reference, bool excludeAlias)
        {
            var formatted = TryFormatAsObjectReference(reference as ObjectReference, excludeAlias)
                            ??
                            TryFormatAsFunctionReference(reference as FunctionReference, excludeAlias)
                            ??
                            TryFormatAsMathReference(reference as MathReference, excludeAlias)
                            ??
                            TryFormatAsAllColumnsReference(reference as AllColumnsSpecialReference, excludeAlias);

            if (formatted != null) return formatted;

            throw new InvalidOperationException("SimpleReference type not supported.");
        }

// ReSharper disable UnusedParameter.Local
        private string TryFormatAsAllColumnsReference(AllColumnsSpecialReference allColumnsSpecialReference, bool excludeAlias)
// ReSharper restore UnusedParameter.Local
        {
            if (ReferenceEquals(allColumnsSpecialReference, null)) return null;
            var table = _schema.FindTable(allColumnsSpecialReference.Table.GetAllObjectNamesDotted());
            var tableName = string.IsNullOrWhiteSpace(allColumnsSpecialReference.GetAlias())
                                ? table.QualifiedName
                                : _schema.QuoteObjectName(allColumnsSpecialReference.GetAlias());
            return string.Format("{0}.*", tableName);
        }

        private string FormatObject(object obj)
        {
            var reference = obj as SimpleReference;
            return reference != null ? FormatColumnClause(reference) : _commandBuilder.AddParameter(obj).Name;
        }

        private string TryFormatAsMathReference(MathReference mathReference, bool excludeAlias)
        {
            if (ReferenceEquals(mathReference, null)) return null;

            if (excludeAlias || mathReference.GetAlias() == null)
            {
                return string.Format("({0} {1} {2})", FormatObject(mathReference.LeftOperand),
                                     MathOperatorToString(mathReference.Operator),
                                     FormatObject(mathReference.RightOperand));
            }

            return string.Format("({0} {1} {2}) AS {3}", FormatObject(mathReference.LeftOperand),
                                 MathOperatorToString(mathReference.Operator),
                                 FormatObject(mathReference.RightOperand),
                                 _schema.QuoteObjectName(mathReference.GetAlias()));

        }

        private string MathOperatorToString(MathOperator @operator)
        {
            switch (@operator)
            {
                case MathOperator.Add:
                    return _schema.Operators.Add;
                case MathOperator.Subtract:
                    return _schema.Operators.Subtract;
                case MathOperator.Multiply:
                    return _schema.Operators.Multiply;
                case MathOperator.Divide:
                    return _schema.Operators.Divide;
                case MathOperator.Modulo:
                    return _schema.Operators.Modulo;
                default:
                    throw new InvalidEnumArgumentException("Invalid MathOperator specified.");
            }
        }

        private string TryFormatAsFunctionReference(FunctionReference functionReference, bool excludeAlias)
        {
            if (ReferenceEquals(functionReference, null)) return null;

            var sqlName = _functionNameConverter.ConvertToSqlName(functionReference.Name);
            string formatted;

            if (sqlName.Equals("countdistinct", StringComparison.OrdinalIgnoreCase))
            {
                formatted = string.Format("count(distinct {0})", FormatColumnClause(functionReference.Argument));
            }
            else
            {
                formatted = string.Format("{0}({1}{2})", sqlName,
                                     FormatColumnClause(functionReference.Argument),
                                     FormatAdditionalArguments(functionReference.AdditionalArguments));
            }

            if ((!excludeAlias) && functionReference.GetAlias() != null)
            {
                formatted = string.Format("{0} AS {1}", formatted, _schema.QuoteObjectName(functionReference.GetAlias()));
            }
            return formatted;
        }

        private string FormatAdditionalArguments(IEnumerable<object> additionalArguments)
        {
            StringBuilder builder = null;
            foreach (var additionalArgument in additionalArguments)
            {
                if (builder == null) builder = new StringBuilder();
                builder.AppendFormat(",{0}", _commandBuilder.AddParameter(additionalArgument));
            }
            return builder != null ? builder.ToString() : string.Empty;
        }

        private string TryFormatAsObjectReference(ObjectReference objectReference, bool excludeAlias)
        {
            if (ReferenceEquals(objectReference, null)) return null;

            var table = _schema.FindTable(objectReference.GetOwner().GetAllObjectNamesDotted());
            var tableName = string.IsNullOrWhiteSpace(objectReference.GetOwner().GetAlias())
                                ? table.QualifiedName
                                : _schema.QuoteObjectName(objectReference.GetOwner().GetAlias());
            var column = table.FindColumn(objectReference.GetName());
            if (excludeAlias || objectReference.GetAlias() == null)
            {
                return string.Format("{0}.{1}", tableName, column.QuotedName);
            }

            return string.Format("{0}.{1} AS {2}", tableName, column.QuotedName,
                                 _schema.QuoteObjectName(objectReference.GetAlias()));
        }

    }
}