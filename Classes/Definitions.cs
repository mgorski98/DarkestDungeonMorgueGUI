

using System.Collections.Generic;
using System.Collections.ObjectModel;

public enum HeroClass : byte {
    Abomination = 0,
    Antiquarian,
    Arbalest,
    Bounty_Hunter,
    Crusader,
    Flagellant,
    Grave_Robber,
    Hellion,
    Highwayman,
    Hound_Master,
    Jester,
    Leper,
    Man_At_Arms,
    Occultist,
    Plague_Doctor,
    Shieldbreaker,
    Vestal
}

public enum QuirkType : byte {
    Positive = 0,
    Negative
}

public enum Affliction : byte {
    None = 0,
    Fearful,
    Paranoid,
    Selfish,
    Masochistic,
    Abusive,
    Hopeless,
    Irrational, 
    Rapturous,
    Refracted
}

public enum Virtue : byte {
    None = 0,
    Stalwart,
    Courageous,
    Focused,
    Powerful,
    Vigorous
}

public enum HeroLevel : byte {
    Seeker = 0,
    Apprentice,
    Adventurer,
    Veteran,
    Master,
    Champion,
    Legend
}

public enum HeroesSortType : byte {
    Name,
    Level,
    Class
}

public static class ObservableCollectionExtensions {
    
    public static void AddRange<T>(this ObservableCollection<T> observable, List<T> other) {
        if (other == null) return;
        foreach (var obj in other) {
            observable.Add(obj);
        }
    }

}
