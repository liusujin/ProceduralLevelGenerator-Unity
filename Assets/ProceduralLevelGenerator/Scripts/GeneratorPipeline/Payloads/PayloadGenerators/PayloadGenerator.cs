﻿namespace Assets.ProceduralLevelGenerator.Scripts.GeneratorPipeline.Payloads.PayloadGenerators
{
	using System;
	using System.Linq;
	using RoomTemplates.TilemapLayers;
	using UnityEngine;
	using UnityEngine.Tilemaps;
	using Random = System.Random;

	[CreateAssetMenu(menuName = "Dungeon generator/Payload generator", fileName = "PayloadGenerator")]
	public class PayloadGenerator : AbstractPayloadGenerator
	{
		public AbstractTilemapLayersHandler TilemapLayersHandler;

		public bool UseRandomSeed = true;

		public int RandomGeneratorSeed;

		public bool PrintUsedSeed = true;

		private static readonly string GameObjectName = "Generated dungeon";

		private readonly Random seedsGenerator = new Random();

		public override object InitializePayload()
		{
			var gameHolderOld = GameObject.Find(GameObjectName);

			if (gameHolderOld != null)
			{
				DestroyImmediate(gameHolderOld);
			}

			var gridObject = new GameObject(GameObjectName);
			gridObject.AddComponent<Grid>();

			TilemapLayersHandler.InitializeTilemaps(gridObject);

			var seed = UseRandomSeed ? seedsGenerator.Next() : RandomGeneratorSeed;

			if (PrintUsedSeed)
			{
				Debug.Log($"Random generator seed: {seed}");
			}

			return new PipelinePayload()
			{
				Tilemaps = gridObject.GetComponentsInChildren<Tilemap>().ToList(),
				GameObject = gridObject,
				Random = new Random(seed),
			};
		}
	}
}