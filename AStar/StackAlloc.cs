using System;
using System.Collections.Generic;


public class StackAlloc{


    public static unsafe void UsingStackAlloc( int size)
    {
        int* p = stackalloc int[size];
        for (int i = 0; i < size; i++)
        {
            p[i] = i;
        }

     
    }

    public static void UsingStackAllocSpan( int size){

        Span<int> p = stackalloc int[size];
        for (int i = 0; i < size; i++)
        {
            p[i] = i;
        }
    }

    public static void UsingNoStack( int size)
    {
        int[] p = new int[size];
        for (int i = 0; i < size; i++)
        {
            p[i] = i;
        }

    }


}