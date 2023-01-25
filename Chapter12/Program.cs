using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter12
{
    //사용자 정의 예외 처리도 아래와 같이 가능하다!
    class InvalidArgumentException : Exception
    {
        public InvalidArgumentException() 
        { 
        }
        public InvalidArgumentException(string message) : base(message)
        {
        }
        public object Argument
        {
            get;set;
        }
        public string Range
        {
            get;set;
        }
    }
    class FilterableException : Exception
    {
        public int ErrorNo { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            subCh02();
            Console.WriteLine();
            subCh04();
            Console.WriteLine();
            subCh04_01();
            Console.WriteLine();
            subCh05();
            Console.WriteLine();
            subCh06();
            Console.WriteLine(); 
            subCh07();
            Console.WriteLine();
            subCh08();
            Console.WriteLine();
        }

        static void subCh02()
        {
            try
            {
                int[] arr = { 1, 2, 3 };
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(arr[i]);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine($"예외가 발생했습니다. : {e.Message}");
            }

            Console.WriteLine("종료");
        }
        static void doSomething(int arg)
        {
            if (arg < 10) Console.WriteLine($"args : {arg}");
            else throw new Exception("arg가 10보다 큽니다.");
        }
        static void subCh04()
        {
            try
            {
                doSomething(1);
                doSomething(3);
                doSomething(5);
                doSomething(9);
                doSomething(11);
                doSomething(15);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void subCh04_01()
        {
            try
            {
                int? a = null;
                int b = a ?? throw new ArgumentNullException();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
            }

            try
            {
                int[] array = new[] { 1, 2, 3 };
                int idx = 4;
                int val = array[idx >= 0 && idx < 3 ? idx :
                    throw new IndexOutOfRangeException()];
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
        }
        //try-catch 문에서 마지막에 오는 finally 구문은 항상 실행한다고 생각!
        static int Divide(int dividend, int divisor)
        {
            try
            {
                Console.WriteLine("Divide() 시작");
                return dividend / divisor;
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine("Divide() 예외 발생");
                throw e;
            }
            finally
            {
                Console.WriteLine("Divide() 끝!");
            }
        }
        static void subCh05()
        {
            try
            {
                Console.Write("제수를 입력하세요: ");
                String tmp = Console.ReadLine();
                int dividend = Convert.ToInt32(tmp);

                Console.Write("피제수를 입력하세요: ");
                tmp = Console.ReadLine();
                int divisor = Convert.ToInt32(tmp);

                Console.WriteLine("{0}/{1} = {2}",
                    dividend, divisor, Divide(dividend, divisor));
            }
            catch (FormatException e)
            {
                Console.WriteLine("에러: " + e.Message);
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine("에러: " + e.Message);
            }
            finally
            {
                Console.WriteLine("프로그램을 종료합니다.");
            }
        }
        //사용자 정의 예외 만들기
        static uint MergeARGB(uint alpha, uint red, uint green, uint blue)
        {
            uint[] args = new uint[] { alpha, red, green, blue };
            foreach (uint arg in args)
            {
                if (arg > 255)
                {
                    throw new InvalidArgumentException()
                    {
                        Argument = arg,
                        Range = "0-255"
                    };
                
                }
            }
            return (alpha << 24 & 0xFF000000) | (red << 16 & 0x00FF0000)
                | (green << 8 & 0x0000FF00) | (blue & 0x000000FF);

        }
        static void subCh06()
        {
            try
            {
                Console.WriteLine("0x{0:X}", MergeARGB(255, 111, 111, 111));
                Console.WriteLine("0x{0:X}", MergeARGB(1, 65, 192, 128));
                Console.WriteLine("0x{0:X}", MergeARGB(0, 255, 255, 300));
            }
            catch (InvalidArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Argument: {e.Argument}, Range: {e.Range}");
            }
        }
        //예외 필터링 -> when을 사용하는데 if 라고 생각하면 됨!
        static void subCh07()
        {
            Console.WriteLine("Enter Number Between 0-10");
            string input = Console.ReadLine();
            try
            {
                int num = Int32.Parse(input);
                if (num < 0 || num > 10) throw new FilterableException { ErrorNo = num };
                else Console.WriteLine($"Output: {num}");
            }
            catch(FilterableException e) when (e.ErrorNo < 0)
            {
                Console.WriteLine("Negative input is not allowed");
            }
            catch(FilterableException e) when (e.ErrorNo > 10)
            {
                Console.WriteLine("Too big number is not allowed.");
            }
                
        }
        static void subCh08()
        {
            try
            {
                int a = 1;
                Console.WriteLine(3 / --a);
            }
            catch(DivideByZeroException e)
            {
                //디버깅에서 용이하게 사용되는 stacktrace 기능 사용법!
                Console.WriteLine(e.StackTrace);
            }
        }

    }
}
