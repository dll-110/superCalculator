using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace superCalculator
{
    class Calculator
    {
        Stack<string> stack = new Stack<string>();
        public decimal? Calculate(string expr)
        {
            
            StringBuilder num = new StringBuilder();
            string @operator = "";

            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];
                if (c == ' ') continue;

                if (char.IsDigit(c))
                {
                    num.Append(c);
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    if (@operator != "")
                    {
                        CalculateHelper(stack, @operator, num);
                    }
                    @operator = c.ToString();
                    num = new StringBuilder();
                }
                else if (c == '(')
                {
                    stack.Push(c.ToString());
                }
                else if (c == ')')
                {
                    while (stack.Peek() != "(")
                    {
                        CalculateHelper(stack, @operator, num);
                    }
                    stack.Pop();
                }
            }
            if (@operator != "")
            {
                CalculateHelper(stack, @operator, num);
            }

            decimal? result = null;
            while (stack.Count > 0)
            {
                result = Operate(stack, stack.Pop(), result ?? 0);
                if (result == null) return null;
            }
            return result;
        }

        void CalculateHelper(Stack<string> stack, string @operator, StringBuilder num)
        {
            stack.Push(num.ToString());
            stack.Push(@operator);
            num = new StringBuilder();
        }

        decimal? Operate(Stack<string> stack, string @operator, decimal b)
        {
            decimal a = decimal.Parse(stack.Pop());
            switch (@operator)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "*": return a * b;
                case "/": return a / b;
                default:
                    return null;
            }
        }
    }
}
