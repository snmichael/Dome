using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Based
{
    /// <summary>
    /// 缓存更新操作结构体基类
    /// Object 缓存适用
    /// </summary>
    [Serializable]
    public class TAttribute
    {
        /// <summary>
        /// 内聚类,可序列化
        /// </summary>
        [Serializable]
        public class IProperty
        {
            /// <summary>
            /// 属性值
            /// </summary>
            private object _Value;

            /// <summary>
            /// 属性值
            /// </summary>
            public object Value
            {
                get
                {
                    bool flag = false;
                    object value;
                    try
                    {
                        Monitor.Enter(this, ref flag);
                        value = this._Value;
                    }
                    finally
                    {
                        if (flag)
                        {
                            Monitor.Exit(this);
                        }
                    }
                    return value;
                }
                set
                {
                    bool flag = false;
                    try
                    {
                        Monitor.Enter(this, ref flag);
                        this._Value = value;
                    }
                    finally
                    {
                        if (flag)
                        {
                            Monitor.Exit(this);
                        }
                    }
                }
            }

            /// <summary>
            /// 内聚类,可序列化
            /// </summary>
            /// <param name="eValue">属性值</param>
            public IProperty(object eValue)
            {
                this._Value = eValue;
            }
        }

        /// <summary>
        /// 装载属性的Hashtable
        /// </summary>
        private Hashtable HTable;

        /// <summary>
        /// 属性列表
        /// </summary>
        private string[] Columns;

        /// <summary>
        /// 获得当前string值
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="dString">默认string</param>
        /// <returns>string</returns>
        public string this[string Key, string dString]
        {
            get
            {
                object obj = this[Key, null, true];
                string result;
                if (obj == null)
                {
                    result = dString;
                }
                else
                {
                    result = obj.ToString();
                }
                return result;
            }
        }

        /// <summary>
        /// 获得当前int值
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="dInt">默认int</param>
        /// <returns>int</returns>
        public int this[string Key, int dInt]
        {
            get
            {
                object obj = this[Key, null, true];
                int result;
                if (obj == null)
                {
                    result = dInt;
                }
                else
                {
                    result = Base.ToInt(obj.ToString(), dInt);
                }
                return result;
            }
        }

        /// <summary>
        /// 获得当前bool值
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="dBool">默认bool</param>
        /// <returns>bool</returns>
        public bool this[string Key, bool dBool]
        {
            get
            {
                object obj = this[Key, null, true];
                bool result;
                if (obj == null)
                {
                    result = dBool;
                }
                else
                {
                    result = Base.IsBool(obj.ToString(), dBool);
                }
                return result;
            }
        }

        /// <summary>
        /// 获得当前decimal值
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="dDecimal">默认decimal</param>
        /// <returns>decimal</returns>
        public decimal this[string Key, decimal dDecimal]
        {
            get
            {
                object obj = this[Key, null, true];
                decimal result;
                if (obj == null)
                {
                    result = dDecimal;
                }
                else
                {
                    result = Base.ToDecimal(obj.ToString(), dDecimal);
                }
                return result;
            }
        }

        /// <summary>
        /// 获得当前float值
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="dFloat">默认float</param>
        /// <returns>float</returns>
        public float this[string Key, float dFloat]
        {
            get
            {
                object obj = this[Key, null, true];
                float result;
                if (obj == null)
                {
                    result = dFloat;
                }
                else
                {
                    result = Base.ToFloat(obj.ToString(), dFloat);
                }
                return result;
            }
        }

        /// <summary>
        /// 获得当前DateTime值
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="dTime">默认DateTime</param>
        /// <returns>DateTime</returns>
        public DateTime this[string Key, DateTime dTime]
        {
            get
            {
                object obj = this[Key, null, true];
                DateTime result;
                if (obj == null)
                {
                    result = dTime;
                }
                else
                {
                    result = Base.ToTime(obj.ToString(), dTime);
                }
                return result;
            }
        }

        /// <summary>
        /// 获得当前对象IProperty中的Value对象
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="dObject">默认对象</param>
        /// <param name="Flag">标志</param>
        /// <returns>object</returns>
        public object this[string Key, object dObject, bool Flag]
        {
            get { return this[Key].Value ?? dObject; }
        }

        /// <summary>
        /// 获取当前对象
        /// </summary>
        /// <param name="Key">主键</param>
        /// <returns>IObject</returns>
        public TAttribute.IProperty this[string Key]
        {
            get
            {
                TAttribute.IProperty result;
                if (this.HTable.ContainsKey(Key))
                {
                    result = (TAttribute.IProperty) this.HTable[Key];
                }
                else
                {
                    TAttribute.IProperty property = new TAttribute.IProperty(null);
                    this.HTable.Add(Key, property);
                    result = property;
                }
                return result;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.HTable = new Hashtable();
            if (this.Columns != null)
            {
                int num = this.Columns.Length;
                for (int i = 0; i < num; i++)
                {
                    this.Add(this.Columns[i]);
                }
            }
        }

        /// <summary>
        /// 实例化
        /// </summary>
        public TAttribute()
        {
            this.Init();
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="Columns">属性列表</param>
        public TAttribute(string[] Columns)
        {
            this.Columns = Columns;
            this.Init();
        }

        /// <summary>
        /// 新增属性
        /// </summary>
        /// <param name="Key">主键</param>
        public void Add(string Key)
        {
            this.Set(Key, null);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="Key">主键</param>
        /// <param name="Value">值</param>
        public void Set(string Key, object Value)
        {
            this[Key].Value = Value;
        }

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="Key">主键</param>
        public void Remove(string Key)
        {
            if (this.HTable.ContainsKey(Key))
            {
                this.HTable.Remove(Key);
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            this.HTable.Clear();
        }
    }
}
