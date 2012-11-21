using System;
using System.Collections.Generic;
using Glimpse.Core.Configuration;

namespace Glimpse.Core.Extensions
{
    public static class TypeElementCollectionExtensions
    {
         public static IEnumerable<Type> ToEnumerable(this TypeElementCollection collection)
         {
             foreach (TypeElement typeElement in collection)
             {
                 yield return typeElement.Type;
             }
         }
    }
}