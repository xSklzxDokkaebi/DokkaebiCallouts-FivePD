﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FivePD.API;
using FivePD.API.Utils;
using CitizenFX.Core;

namespace DokkaebiCallouts
{
    [CalloutProperties("ANPR: Owner Wanted", "xSklzx Dokkaebi", "1.0.0.0")]
    public class ANPR_OwnerWanted : Callout
    {
        private readonly Random rnd = new Random();
        private Ped suspect;
        private Vehicle vehicle;
        public ANPR_OwnerWanted()
        {
            float offsetX = rnd.Next(100, 700);
            float offsetY = rnd.Next(100, 700);

            InitInfo(World.GetNextPositionOnStreet(new Vector3(offsetX, offsetY, 0)));

            ShortName = "ANPR: Registered Owner Wanted";
            CalloutDescription = "An ANPR camera has picked up a flag on a vehicle, the registered owner is wanted by law enforcement.";
            ResponseCode = 3;
            StartDistance = 100f;
        }

        public override async Task OnAccept()
        {
            InitBlip();
            suspect = await SpawnPed(RandomUtils.GetRandomPed(), Location);
            vehicle = await SpawnVehicle(spawnRandomCar(), Location);

            keepTask(suspect);

            suspect.SetIntoVehicle(vehicle, VehicleSeat.Driver);
        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);

            PedData pedData = new PedData();
            VehicleData vehicleData = new VehicleData();

            string firstname = pedData.FirstName;
            string lastname = pedData.LastName;
            pedData.Warrant = randomWarrant();
            vehicleData.OwnerFirstName = firstname;
            vehicleData.OwnerLastName = lastname;
            vehicleData.Flag = "Registed Owner Wanted";

            int chance = rnd.Next(0, 10);
            if (chance >= 0 && chance <= 3)
            {
                var pursuit = Pursuit.RegisterPursuit(suspect);
                pursuit.Init(true, 30f, 125f, true);
                pursuit.ActivatePursuit();
            }
        }

        public static void keepTask(Ped p)
        {
            p.BlockPermanentEvents = true;
            p.AlwaysKeepTask = true;
        }

        public string randomWarrant()
        {
            List<string> warrantList = new List<string>
            {
                "Search Warrant",
                "Arrest Warrant",
                "Bench Warrant",
                "Failure to Appear",
                "Failure to Pay",
                "Child Support Arrest Warrant"
            };
            return warrantList[rnd.Next(warrantList.Count)];
        }

        private VehicleHash spawnRandomCar()
        {
            List<VehicleHash> vehicles = new List<VehicleHash>
            {
                VehicleHash.Futo,
                VehicleHash.Gauntlet,
                VehicleHash.Gauntlet2,
                VehicleHash.Intruder,
                VehicleHash.Khamelion,
                VehicleHash.Kuruma,
                VehicleHash.Kuruma2,
                VehicleHash.Sentinel,
                VehicleHash.Sentinel2,
                VehicleHash.Schafter2,
                VehicleHash.Schafter3,
                VehicleHash.Schafter4,
                VehicleHash.Schafter5,
                VehicleHash.Schafter6,
                VehicleHash.ZType
            };
            return vehicles[rnd.Next(vehicles.Count)];
        }
    }
}