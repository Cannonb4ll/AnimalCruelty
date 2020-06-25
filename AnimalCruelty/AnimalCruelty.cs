using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.UI.Screen;
using FivePD.API;
using Utilities = AnimalCruelty.Utils.Utilities;

namespace AnimalCruelty
{
    [CalloutProperties("Animal Cruelty", "Dennis Smink", "1.0.0")]
    public class AnimalCruelty : Callout
    {
        Ped suspect, victim;
        
        PedHash[] availableDogs = {
            PedHash.Rottweiler,
            PedHash.Shepherd,
            PedHash.Westy,
            PedHash.Retriever,
            PedHash.Pug,
            PedHash.Poodle,
            PedHash.Husky,
            PedHash.Boar,
        };
        
        public AnimalCruelty()
        {
            Random rnd = new Random();
            float offsetX = rnd.Next(100, 700);
            float offsetY = rnd.Next(100, 700);

            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(new Vector3(offsetX, offsetY, 0))));

            ShortName = "Animal cruelty";
            CalloutDescription = "We've got a call that someone is beating up their dog!";
            ResponseCode = 3;
            StartDistance = 50f;
        }

        public override async Task OnAccept()
        {
            InitBlip();

            // Throw out a few notifications
            ShowNotification("We've received a call that someone is beating up their dog.");
            ShowNotification("Head over there and arrest the owner for animal cruelty.");
            
            suspect = await SpawnPed(GetRandomPed(), Location);
            victim = await SpawnPed(availableDogs[Utilities.RANDOM.Next(0, availableDogs.Length)], Location + 12);

            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            victim.AlwaysKeepTask = true;
            victim.BlockPermanentEvents = true;
        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);

            suspect.AttachBlip();

            if (victim.Handle.GetHashCode().ToString() == PedHash.Boar.ToString())
            {
                ShowSubtitle("Come here you stupid boar!", 5000);
            }
            else
            {
                ShowSubtitle("Come here you stupid dog!", 5000);
            }
            
            suspect.Task.FightAgainst(victim);
            victim.Task.ReactAndFlee(suspect);
        }
    }
}