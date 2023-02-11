using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

namespace Chapter16
{
    class MyClass
    {
        //Attribute - 부가 정보를 기록하고 읽을 수 있는 기능!
        [Obsolete("OldMethod는 폐기되었습니다. NewMethod()를 이용하세요.")]
        public void OldMethod()
        {
            Console.WriteLine("I'm old");
        }
        public void NewMethod()
        {
            Console.WriteLine("I'm new");
        }
    }
    public static class Trace
    {
        public static void WriteLine(string message, [CallerFilePath] string file="",
            [CallerLineNumber]int line = 0, [CallerMemberName] string member = "")
        {
            Console.WriteLine($"{file}(Line: {line}) {member}: {message}");
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    class History : System.Attribute
    {
        private string programmer;
        public double version;
        public string chages;
        public History(string programmer)
        {
            this.programmer = programmer;
            version = 1.0;
            chages = "First release";
        }
        public string GetProgrammer()
        {
            return this.programmer;
        }
    }
    [History("Sean", version = 0.1, chages ="2017-11-01 Created class stub")]
    [History("Bob", version = 0.2, chages ="2020-12-03 Added Func() Method")]
    class MyClass2
    {
        public void Fund()
        {
            Console.WriteLine("Func()");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            subCh01_03();
            subCh02_01();
            subCh02_02();
            subCh02_03();
        }

        static void subCh01_03()
        {
            AssemblyBuilder newAssembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("CalculatorAssembly"), AssemblyBuilderAccess.Run);
            ModuleBuilder newModule = newAssembly.DefineDynamicModule("Calculator");
            TypeBuilder newType = newModule.DefineType("Sum1To100");

            MethodBuilder newMethod = newType.DefineMethod("Calculate",
                MethodAttributes.Public,
                typeof(int),  //반환 형식
                new Type[0]); //매개변수

            ILGenerator generator = newMethod.GetILGenerator();

            generator.Emit(OpCodes.Ldc_I4, 1);

            for(int i=2; i<=100; i++)
            {
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Add);
            }

            generator.Emit(OpCodes.Ret);
            newType.CreateType();

            object sumTo100 = Activator.CreateInstance(newType);

            MethodInfo Calculate = sumTo100.GetType().GetMethod("Calculate");
            Console.WriteLine(Calculate.Invoke(sumTo100, null));
            Console.WriteLine();
        }
        static void subCh02_01()
        {
            MyClass obj = new MyClass();
            obj.OldMethod();
            obj.NewMethod();
            Console.WriteLine();
        }
        static void subCh02_02()
        {
            Trace.WriteLine("즐거운 프로그래밍!");
            Console.WriteLine();
        }
        static void subCh02_03()
        {
            Type type = typeof(MyClass2);
            Attribute[] attributes = Attribute.GetCustomAttributes(type);
            Console.WriteLine("MyClass2 chage history...");
            foreach(Attribute a in attributes)
            {
                History h = a as History;
                if(h!= null)
                {
                    Console.WriteLine("Ver: {0}, Programmer: {1}, Chages: {2}", h.version, h.GetProgrammer(), h.chages);
                }
            }
            Console.WriteLine();
        }
    }


}
