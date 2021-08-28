using System;

namespace KeyValueStorage.Abstractions
{
    [Flags]
    public enum KeyValueStorageCapability
    {
        None = 0,

        // --- FEATURES -----
        Fetch       = 0x01,
        Store       = 0x02,
        List        = 0x04,
        AsyncList   = 0x08,

        AllFeatures = Fetch | Store | List | AsyncList,

        // --- EXTENSIONS ----
        KeyPrefix   = 0x1 << 8,
        Metadata    = 0x2 << 8,
        MultiTenant = 0x4 << 8,

        AllExtensions = KeyPrefix | Metadata | MultiTenant,

        // --- EVENTS ----
        StoreEvents = 0x1 << 12,

        AllEvents   = StoreEvents,

        // --- ALL -------
        All = AllFeatures | AllExtensions | AllEvents,
    }
}