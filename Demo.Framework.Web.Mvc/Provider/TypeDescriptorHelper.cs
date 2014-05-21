using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Demo.Framework.Web.Mvc.Provider{

    public static class TypeDescriptorHelper
    {
        static Hashtable hashtable = new Hashtable();
        static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        static TypeDescriptorHelper()
        {

        }
        public static void RegisterMetadataType(Type type, Type metadataType)
        {
            locker.EnterWriteLock();

            hashtable[type] = metadataType;

            locker.ExitWriteLock();
        }
        public static ICustomTypeDescriptor Get(Type type)
        {
            locker.EnterReadLock();
            var metadataType = hashtable[type] as Type;
            ICustomTypeDescriptor descriptor = null;
            if (metadataType != null)
            {
                descriptor = new AssociatedMetadataTypeTypeDescriptionProvider(type, metadataType).GetTypeDescriptor(type);
            }
            locker.ExitReadLock();
            return descriptor;
        }
    }
}