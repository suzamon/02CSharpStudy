using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Chapter11
{
    //일반화 클래스 -> 데이터 형식을 일반화한 클래스
    //선언하는 방식 -> Class 클래스_이름 <형식_매개변수> {...}
    class MyList<T>
    {
        private T[] array;
        public MyList()
        {
            array = new T[3];
        }
        public T this[int index]
        {
            get { return array[index]; }
            set
            {
                if (index >= array.Length)
                {
                    Array.Resize<T>(ref array, index + 1);
                    Console.WriteLine($"Array Resize: {array.Length}");
                }
                array[index] = value;
            }
        }
        public int Length
        {
            get { return array.Length; }
        }
    }
    //형식 매개변수 제약시키기 -> struct: 값 형식, class: 참조 형식,
    //where T: U ::T는 또다른 매개변수 U로부터 상속받은 클래스여야!
    class StructArray<T> where T : struct
    {
        public T[] Array { get; set; }
        public StructArray(int size)
        {
            Array = new T[size];
        }
    }
    class RefArray<T> where T : class
    {
        public T[] Array { get; set; }
        public RefArray(int size)
        {
            Array = new T[size];
        }
    }
    class Base { };
    class Derived : Base { };
    class BaseArray<U> where U : Base
    {
        public U[] Array { get; set; }
        public BaseArray(int size)
        {
            Array = new U[size];
        }
        public void CopyArray<T>(T[] Source) where T : U
        {
            Source.CopyTo(Array, 0);
        }
    }
    class MainApp
    {
        static void Main(string[] args)
        {
            subCh02();
            Console.WriteLine();
            subCh03();
            Console.WriteLine();
            subCh04();
            Console.WriteLine();
            subCh05();
            Console.WriteLine();
        }
        //일반화 메소드 -> '한정자 반환_형식 메소드이름<형식_매개변수>(매개변수_목록){...}
        //식으로 일반화 메소드 선언하게 되면 아래 응용법과 같이만 사용하면 모든 매개변수 형식에 따라
        //메소드 사용 가능(오버로딩 사용 안해도 됨!)
        static void CopyArray<T>(T[] source, T[] target)
        {
            for(int i=0; i<source.Length; i++)
            {
                target[i] = source[i];
            }
        }
        static void subCh02()
        {
            int[] source = { 1, 2, 3, 4, 5 };
            int[] target = new int[source.Length];

            CopyArray<int>(source, target);
            foreach(int ele in target)
            {
                Console.WriteLine(ele);
            }
            Console.WriteLine();

            string[] source2 = { "one", "two", "three", "four", "five" };
            string[] target2 = new string[source2.Length];
            CopyArray<string>(source2, target2);

            foreach(string ele in target2)
            {
                Console.WriteLine(ele);
            }
        }
        static void subCh03()
        {
            MyList<string> str_list = new MyList<string>();
            str_list[0] = "abc";
            str_list[1] = "def";
            str_list[2] = "ghi";
            str_list[3] = "jkl";
            str_list[4] = "mno";

            for(int i= 0; i<str_list.Length; i++)
            {
                Console.WriteLine(str_list[i]);
            }

            MyList<int> int_list = new MyList<int>();
            int_list[0] = 1;
            int_list[1] = 2;
            int_list[2] = 3;
            int_list[3] = 4;
            int_list[4] = 5;

            for(int i=0; i < int_list.Length; i++)
            {
                Console.WriteLine(int_list[i]);
            }
        }        
        public static T CreateInstance <T>() where T : new()
        {
            return new T();
        }
        static void subCh04()
        {
            StructArray<int> a = new StructArray<int>(3);
            a.Array[0] = 0;
            a.Array[1] = 1;
            a.Array[2] = 2;

            RefArray<StructArray<double>> b = new RefArray<StructArray<double>>(3);
            b.Array[0] = new StructArray<double>(5);
            b.Array[1] = new StructArray<double>(10);
            b.Array[2] = new StructArray<double>(1005);

            BaseArray<Base> c = new BaseArray<Base>(3);
            c.Array[0] = new Base();
            c.Array[1] = new Derived();
            c.Array[2] = CreateInstance<Base>();

            BaseArray<Derived> d = new BaseArray<Derived> (3);
            d.Array[0] = new Derived(); //Base형식은 여기에 할당할 수 없다!
            d.Array[1] = CreateInstance<Derived>();
            d.Array[2] = CreateInstance<Derived>();

            BaseArray<Derived> e = new BaseArray<Derived> (3);
            e.CopyArray<Derived>(d.Array);
        }
        static void subCh05()
        {
            //일반화 컬렉션!! -> 기본으로 제공되는 컬렉션은 모두 object 형식으로 구현
            //나중에 형식을 잘못 넣어서 오류가 날 수도 있다!
            //이를 해결할 수 있는 것이 일반화 컬렉션 -> 선언 시 <매개변수_형식>으로 정의!
            List <int> list = new List<int> ();
            for (int i = 0; i < 5; i++) list.Add(i);
            foreach (int element in list) Console.Write($"{element} ");
            Console.WriteLine();

            Queue<int> q = new Queue<int>();
            q.Enqueue(1);
            q.Enqueue(2);
            q.Enqueue(3);
            while (q.Count > 0)
                Console.WriteLine(q.Dequeue());

            Stack<int> s = new Stack<int>();
            s.Push(1);
            s.Push(2);
            s.Push(3);
            while (s.Count > 0)
                Console.WriteLine(s.Pop());

            Dictionary<string, string> dic = new Dictionary<string, string> ();
        }
    }
}
