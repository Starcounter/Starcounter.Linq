using System;
using System.Linq.Expressions;

namespace Starcounter.Linq.Visitors
{
    //It's cheaper to pass the state around in a stateless visitor than 
    //to create a new visitor per query
    public abstract class StatelessVisitor<T>
    {
        public virtual void Visit(Expression node, T state)
        {
            switch (node)
            {
                case BinaryExpression binary:
                    VisitBinary(binary, state);
                    break;
                case BlockExpression block:
                    VisitBlock(block, state);
                    break;
                case ConditionalExpression cond:
                    VisitConditional(cond,state);
                    break;
                case ConstantExpression @const:
                    VisitConstant(@const,state);
                    break;
                case DebugInfoExpression dbg:
                    VisitDebugInfo(dbg,state);
                    break;
                case DynamicExpression dyn:
                    VisitDynamic(dyn,state);
                    break;
                case DefaultExpression def:
                    VisitDefault(def,state);
                    break;
                case InvocationExpression inv:
                    VisitInvocation(inv,state);
                    break;
                case MemberExpression mem:
                    VisitMember(mem,state);
                    break;
                case IndexExpression ind:
                    VisitIndex(ind,state);
                    break;
                case MethodCallExpression call:
                    VisitMethodCall(call,state);
                    break;
                case NewArrayExpression arr:
                    VisitNewArray(arr,state);
                    break;
                case NewExpression @new:
                    VisitNew(@new,state);
                    break;
                case ParameterExpression param:
                    VisitParameter(param,state);
                    break;
                case RuntimeVariablesExpression run:
                    VisitRuntimeVariables(run,state);
                    break;
                case UnaryExpression unary:
                    VisitUnary(unary,state);
                    break;
                case LambdaExpression lam:
                    VisitLambda(lam,state);
                    break;
                case TypeBinaryExpression typ:
                    VisitTypeBinary(typ, state);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public virtual void VisitBinary(BinaryExpression node, T state)
        {
            Visit(node.Left, state);
            Visit(node.Right, state);
        }

        public virtual void VisitTypeBinary(TypeBinaryExpression node, T state)
        {
            throw new NotSupportedException();
        }

        public virtual void VisitBlock(BlockExpression node, T state)
        {
            foreach (var exp in node.Expressions)
            {
                Visit(exp, state);
            }
            foreach (var exp in node.Variables)
            {
                Visit(exp, state);
            }
            Visit(node.Result,state);
        }

        public virtual void VisitConditional(ConditionalExpression node, T state)
        {
            Visit(node.Test, state);
            Visit(node.IfTrue, state);
            Visit(node.IfFalse, state);
        }

        public virtual void VisitConstant(ConstantExpression node, T state)
        {
        }

        public virtual void VisitDebugInfo(DebugInfoExpression node, T state)
        {
        }

        public virtual void VisitDynamic(DynamicExpression node, T state)
        {
        }

        public virtual void VisitDefault(DefaultExpression node, T state)
        {
        }

        public virtual void VisitInvocation(InvocationExpression node, T state)
        {
            Visit(node.Expression,state);
            foreach (var exp in node.Arguments)
            {
                Visit(exp,state);
            }
        }


        public virtual void VisitLambda(LambdaExpression node, T state)
        {
            foreach (var exp in node.Parameters)
            {
                Visit(exp, state);
            }
            Visit(node.Body, state);
        }

        public virtual void VisitMember(MemberExpression node, T state)
        {
           Visit(node.Expression,state);
        }

        public virtual void VisitIndex(IndexExpression node, T state)
        {
            foreach (var exp in node.Arguments)
            {
                Visit(exp,state);
            }
            Visit(node.Object, state);
        }

        public virtual void VisitMethodCall(MethodCallExpression node, T state)
        {
            foreach (var exp in node.Arguments)
            {
                Visit(exp,state);
            }
            Visit(node.Object,state);
        }

        public virtual void VisitNewArray(NewArrayExpression node, T state)
        {
            foreach (var exp in node.Expressions)
            {
                Visit(exp,state);
            }
        }

        public virtual void VisitNew(NewExpression node, T state)
        {
            foreach (var exp in node.Arguments)
            {
                Visit(exp, state);
            }
        }

        public virtual void VisitParameter(ParameterExpression node, T state)
        {
        }

        public virtual void VisitRuntimeVariables(RuntimeVariablesExpression node, T state)
        {
            foreach (var exp in node.Variables)
            {
                Visit(exp,state);
            }
        }

        public virtual void VisitUnary(UnaryExpression node, T state)
        {
            Visit(node.Operand,state);
        }
    }
}
