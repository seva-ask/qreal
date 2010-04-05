﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using ObjectTypes;

namespace QReal.Types
{
    public class DiagramWithTypes
    {
        private Type mDiagram;
        private Dictionary<string, Type> mTypes;

        public DiagramWithTypes(Type diagram, Dictionary<string, Type> types)
        {
            mDiagram = diagram;
            mTypes = types;
        }

        //public ObjectType this[string typeName]
        //{
        //    get
        //    {
        //        return (ObjectType)Activator.CreateInstance(mTypes[typeName]);
        //    }
        //}

        public Type this[string typeName]
        {
            get
            {
                return mTypes[typeName];
            }
        }

        public IEnumerable<string> TypeList
        {
            get
            {
                return mTypes.Keys;
            }
        }
    }

}