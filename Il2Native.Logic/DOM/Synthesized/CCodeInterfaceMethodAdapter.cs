﻿namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Linq;
    using DOM2;

    using Il2Native.Logic.DOM.Implementations;

    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceMethodAdapter : CCodeMethodDeclaration
    {
        public CCodeInterfaceMethodAdapter(ITypeSymbol type, IMethodSymbol interfaceMethod, IMethodSymbol classMethod)
            : base(interfaceMethod)
        {
            var receiver = type == classMethod.ContainingType
                               ? (Expression)new ThisReference { Type = classMethod.ContainingType }
                               : (Expression)new BaseReference { Type = classMethod.ContainingType, ExplicitType = true };

            var call = new Call { ReceiverOpt = receiver, Method = classMethod };
            foreach (var argument in interfaceMethod.Parameters.Select(parameterSymbol => new Parameter { ParameterSymbol = parameterSymbol }))
            {
                call.Arguments.Add(argument);
            }

            MethodBodyOpt = new MethodBody();

            if (classMethod.IsGenericMethod)
            {
                // set generic types
                foreach (var typeArgument in classMethod.TypeArguments)
                {
                    MethodBodyOpt.Statements.Add(
                        new TypeDef { TypeExpression = new TypeExpression { Type = new TypeImpl { SpecialType = SpecialType.System_Object } }, Local = new Local { CustomName = typeArgument.Name } });
                }
            }

            MethodBodyOpt.Statements.Add(!interfaceMethod.ReturnsVoid
                        ? (Statement)new ReturnStatement { ExpressionOpt = call }
                        : (Statement)new ExpressionStatement { Expression = call });
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(string.Format("// adapter: {0}", this.Method));
            c.NewLine();

            c.WriteMethodReturn(this.Method, true);
            c.WriteMethodName(this.Method, allowKeywords: false);
            c.WriteMethodPatameters(this.Method, true, this.MethodBodyOpt != null);

            if (this.MethodBodyOpt == null)
            {
                c.EndStatement();
            }
            else
            {
                this.MethodBodyOpt.WriteTo(c);
            }
        }
    }
}