using Inheritance.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects {

    /***************** Интерфейсы *****************/

    public interface IOwner { // интерфейс IOwner
        int Owner { get; set; }
    }

    public interface IArmy { // интерфейс IArmy
        Army Army { get; set; }
    }

    public interface ITreasure { // интерфейс ITreasure
        Treasure Treasure { get; set; }
    }

    /***************** Классы *****************/
    public class Dwelling : IOwner { // жилище
        public int Owner { get; set; }
    }

    public class Mine : IOwner, IArmy, ITreasure { // рудник
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IArmy, ITreasure { // монстры
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IArmy { // волки
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasure { // месторождение ресурсов
        public Treasure Treasure { get; set; }
    }

    /***************** Взаимодействие *****************/

    public static class Interaction { // класс взаимодействия игрока и объектов на карте
        public static void Make(Player player, object mapObject) { 

            if (mapObject is Dwelling dwellingObj) { // объект на карте является жилищем
                dwellingObj.Owner = player.Id; // присваиваем игроку
                return;
            }

            if (mapObject is Mine mineObj) { // объект на карте является рудником
                if (player.CanBeat(mineObj.Army)) { // игрок может победить монстров
                    mineObj.Owner = player.Id; // присваиваем игроку рудник
                    player.Consume(mineObj.Treasure); // получаем сокровища
                } else player.Die(); //  игрок не может победить монстров, игрок умирает
                return;
            }

            if (mapObject is Creeps creepsObj) { //  объект на карте является монстрами
                if (player.CanBeat(creepsObj.Army)) //  игрок может победить монстров
                    player.Consume(creepsObj.Treasure); // игрок получает сокровища
                else
                    player.Die(); // игрок не может победить монстров, игрок умирает
                return;
            }

            if (mapObject is ResourcePile resourceObj) { // объект на карте является местом добычи ресурсов
                player.Consume(resourceObj.Treasure); // игрок получает сокровища
                return;
            }

            if (mapObject is Wolves wolvesObj) { //  объект на карте является волками
                if (!player.CanBeat(wolvesObj.Army)) //  игрок не может победить волков
                    player.Die(); // игрок умирает
            }
        }
    }
}
