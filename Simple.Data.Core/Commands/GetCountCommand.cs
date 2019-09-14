﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Simple.Data.Commands
{
    class GetCountCommand : ICommand
    {
        /// <summary>
        /// Determines whether the instance is able to handle the specified method.
        /// </summary>
        /// <param name="method">The method name.</param>
        /// <returns>
        /// 	<c>true</c> if the instance is able to handle the specified method; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCommandFor(string method)
        {
            return method.Equals("getcount", StringComparison.InvariantCultureIgnoreCase) || method.Equals("get_count", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="dataStrategy">The data strategy.</param>
        /// <param name="table"></param>
        /// <param name="binder">The binder from the <see cref="DynamicTable"/> method invocation.</param>
        /// <param name="args">The arguments from the <see cref="DynamicTable"/> method invocation.</param>
        /// <returns></returns>
        public object Execute(DataStrategy dataStrategy, DynamicTable table, InvokeMemberBinder binder, object[] args)
        {
            var query = new SimpleQuery(dataStrategy, table.GetQualifiedName());

            if (args.Length == 1)
            {
                var expression = args[0] as SimpleExpression;
                if (expression != null)
                {
                    return query.Where(expression).Count();
                }
                else
                {
                    throw new BadExpressionException("GetCount expects an expression or no parameters.");
                }
            }
            else if (args.Length == 0)
            {
                return query.Count();
            }

            throw new ArgumentException("GetCount expects an expression or no parameters.");
        }
    }
}
