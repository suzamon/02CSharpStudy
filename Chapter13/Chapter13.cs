using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Chapter13
{
    delegate int MyDelegate(int a, int b);
    class Calculator
    {
        public int Plus(int a, int b)
        {
            return a + b;
        }
        public int Minus(int a, int b)
        {
            return a - b;
        }
    }
    delegate int Compare(int a, int b);
    delegate int Compare2<T>(T a, T b);

    //Notify 대리자 선언
    delegate void Notify(string msg);
    //Notify 대리자의 인스턴스 EventOccured를 가지는 클래스 Notifier 선언
    class Notifier
    {
        public Notify EventOccured;
    }
    class EventListener
    {   
        private string name;
        public EventListener(string name)
        {
            this.name = name;
        }

        public void SomethingHappend(string msg)
        {
            Console.WriteLine($"{name}.SomethingHappend: {msg}");
        }
        
    }

    delegate void EventHandler(string message);
    class MyNotifier
    {
        public event EventHandler SomethingHappend;
        public void DoSomething(int number)
        {
            int temp = number % 10;
            if(temp != 0 && temp % 3 == 0)
            {
                SomethingHappend(String.Format("{0} : 짝", number));
            }
        }
    }
    class Chapter13
    {
        static void Main(string[] args)
        {
            subCh01();
            Console.WriteLine();
            subCh02();
            Console.WriteLine();
            subCh03();
            Console.WriteLine();
            subCh04();
            Console.WriteLine();
            subCh05();
            Console.WriteLine();
            subCh06();
            Console.WriteLine();
        }
        //대리자 -> 이벤트가 생겼을 때 알아서 해주는 용도
        //사용 방법
        //1. 대리자를 선언한다
        //2. 대리자의 인스턴스를 생성한다. 인스턴스를 생성할 때는 대리자가 참조할 메소드를 인수로 넘긴다.
        //3. 대지자를 호출한다.
        static void subCh01()
        {
            Calculator Calc = new Calculator();
            MyDelegate Callback;

            Callback = new MyDelegate(Calc.Plus);
            Console.WriteLine(Callback(3, 4));

            Callback = new MyDelegate(Calc.Minus);
            Console.WriteLine(Callback(7, 5));
        }
        //대리자가 사용되는 경우 -> 메소드를 참조할 대리자를 매개변수에 받아서 구현!
        static int AscendCompare(int a, int b)
        {
            if (a > b) return 1;
            else if (a == b) return 0;
            else return -1;
        }
        static int DescendCompare(int a, int b)
        {
            if (a < b) return 1;
            else if (a == b) return 0;
            else return -1;
        }
        static void BubbleSort(int[] DataSet, Compare Comparer)
        {
            int i = 0, j = 0, tmp = 0 ;
            for(i=0;i<DataSet.Length;i++)
            {
                for(j=0; j<DataSet.Length - (i+1); j++)
                {
                    if (Comparer(DataSet[j], DataSet[j + 1]) > 0)
                    {
                        tmp = DataSet[j + 1];
                        DataSet[j+1] = DataSet[j];
                        DataSet[j] = tmp;
                    }
                }
            }
        }
        static void subCh02()
        {
            int[] arr = { 3, 7, 4, 2, 10 };

            Console.WriteLine("Sorting ascending...");
            BubbleSort(arr, new Compare(AscendCompare));

            for(int i=0; i<arr.Length; i++)
            {
                Console.Write($"{arr[i]} ");
            }
            Console.WriteLine();

            int[] arr2 = { 7, 2, 8, 10, 11 };
            Console.WriteLine("Sorting descending...");
            BubbleSort(arr2, new Compare(DescendCompare));
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"{arr2[i]} ");
            }
            Console.WriteLine();
        }
        //대리자도 일반화가능! But 형식 매개변수 이용해서 선언해야 가능!
        static int AscendCompare2<T>(T a, T b) where T: IComparable<T>
        {
            return a.CompareTo(b);
        }
        static int DscendCompare2<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) * -1;
        }
        static void BubbleSort2<T>(T[] DataSet, Compare2<T> Comparer)
        {
            int i = 0, j = 0;
            T tmp;
            for (i = 0; i < DataSet.Length; i++)
            {
                for (j = 0; j < DataSet.Length - (i + 1); j++)
                {
                    if (Comparer(DataSet[j], DataSet[j + 1]) > 0)
                    {
                        tmp = DataSet[j + 1];
                        DataSet[j + 1] = DataSet[j];
                        DataSet[j] = tmp;
                    }
                }
            }

        }
        static void subCh03()
        {
            int[] arr = { 3, 7, 4, 2, 10 };
            Console.WriteLine("Sorting ascending...");
            BubbleSort2<int>(arr, new Compare2<int>(AscendCompare2));

            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"{arr[i]} ");
            }
            Console.WriteLine();

            string[] arr2 = { "abc", "def", "ghi", "jkl", "mno" };
            Console.WriteLine("Sorting descending...");
            BubbleSort2<string>(arr2, new Compare2<string>(DscendCompare2));
            for (int i = 0; i < arr2.Length; i++)
            {
                Console.Write($"{arr2[i]} ");
            }
            Console.WriteLine();
        }
        //대리자 체인 +=/-=/+/-/=으로 이어 붙일 수 있다!
        static void subCh04()
        {
            Notifier noti = new Notifier();
            EventListener listner1 = new EventListener("Listner1");
            EventListener listner2 = new EventListener("Listner2");
            EventListener listner3 = new EventListener("Listner3");

            noti.EventOccured += listner1.SomethingHappend;
            noti.EventOccured += listner2.SomethingHappend;
            noti.EventOccured += listner3.SomethingHappend;
            noti.EventOccured("You've got email.");

            Console.WriteLine();

            noti.EventOccured -= listner2.SomethingHappend;
            noti.EventOccured("Download complete.");
            Console.WriteLine();

            noti.EventOccured = new Notify(listner2.SomethingHappend) + new Notify(listner3.SomethingHappend);
            noti.EventOccured("Nuclear launch detected.");
            Console.WriteLine() ;

            Notify notify1 = new Notify(listner1.SomethingHappend);
            Notify notify2 = new Notify(listner2.SomethingHappend);

            noti.EventOccured = (Notify)Delegate.Combine(notify1, notify2);
            noti.EventOccured("Fire!!");
            Console.WriteLine();

            noti.EventOccured = (Notify)Delegate.Remove(noti.EventOccured, notify2);
            noti.EventOccured("RPG!");
        }
        //익명 메소드 -> delegate를 이용하면 이름이 없는 메소드 구현 가능!
        static void subCh05()
        {
            int[] arr = { 3, 7, 4, 2, 10 };
            Console.WriteLine("Sorting ascending...");
            BubbleSort(arr, delegate (int a, int b) //익명 메소드!
            {
                if (a > b) return 1;
                else if (a == b) return 0;
                else return -1;
            });
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"{arr[i]} ");
            }
            Console.WriteLine();

            int[] arr2 = { 7, 2, 8, 10, 11 };
            Console.WriteLine("Sorting dscending...");
            BubbleSort(arr2, delegate (int a, int b)
            {
                if (a < b) return 1;
                else if(a==b) return 0;
                else return -1;
            });

            for (int i = 0; i < arr2.Length; i++)
            {
                Console.Write($"{arr2[i]} ");
            }
            Console.WriteLine();

        }
        //이벤트: 객체에 일어난 사건 알리기! -> 어떤 일이 생겼을 때 이를 알려주는 객체라고 생각!
        //사용하는 절차
        //1. 대리자를 선언한다(클래스 밖에서나 안에서나 선언해도 상관없음)
        //2. 클래스 내에 1에서 선언한 대리자의 인스턴스를 event 한정자로 수식해서 선언한다.
        //3. 이벤트 핸들러를 작성한다.(이벤트 핸들로는 1에서 선언한 대리자와 일치하는 메소드면 된다.)
        //4. 클래스의 인스턴스를 생성하고 이 객체의 이벤트에서 3에서 작성한 이벤트 핸들러를 등록한다.
        //5. 이벤트가 발생하면 이벤트 핸들러가 호출된다.
        static public void MyHandler(string message)
        {
            Console.WriteLine(message); 
        }
        static void subCh06()
        {
            MyNotifier notifier= new MyNotifier();
            notifier.SomethingHappend += new EventHandler(MyHandler);
            for(int i = 1; i < 30; i++)
            {
                notifier.DoSomething(i);
            }
        }

    }
}
