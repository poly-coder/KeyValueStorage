using System;

namespace KeyValueStorage.Abstractions
{
    /// <summary>
    /// |             | Metadata | KeyPrefix |
    /// | Fetch       |    X     |           |
    /// | Store       |    X     |           | 
    /// | List        |    X     |    X      |
    /// | AsyncList   |    X     |    X      |
    /// | StoreEvents |    X     |    X      |
    /// </summary>
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

        // --- EVENTS ----
        StoreEvents = 0x1 << 8,

        AllEvents   = StoreEvents,

        // --- EXTENSIONS ----
        KeyPrefix   = 0x1 << 12,
        Metadata    = 0x2 << 12,

        AllExtensions = KeyPrefix | Metadata,

        // --- ALL -------
        All = AllFeatures | AllEvents | AllExtensions,
    }
}