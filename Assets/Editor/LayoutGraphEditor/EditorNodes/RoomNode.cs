﻿namespace Assets.Editor.LayoutGraphEditor.EditorNodes
{
	using System;
	using NodeBasedEditor;
	using Scripts.Data.Graphs;
	using UnityEditor;
	using UnityEngine;

	public class RoomNode : IEditorNode<Room>
	{
		public Room Data { get; set; }

		public Action<RoomNode> OnDelete;

		public Action<RoomNode, Event> OnStartConnection;

		public Action<RoomNode, Event> OnEndConnection;

		public Rect Rect;

		public bool IsConnectionMade;

		public EditorMode Mode;

		private readonly GUIStyle style;

		private bool isDragged;


		public RoomNode(Room data, float width, float height, GUIStyle style, EditorMode mode)
		{
			Data = data;
			this.style = style;
			Rect = new Rect(Data.Position.x, Data.Position.y, width, height);
			this.Mode = mode;
		}

		// TODO: refactor
		public bool ProcessEvents(Event e)
		{
			switch (e.type)
			{
				case EventType.MouseDown:
					if (e.button == 1)
					{
						if (Rect.Contains(e.mousePosition))
						{
							ProcessContextMenu();
							e.Use();
						}
					}
					else if (e.button == 0 && Mode == EditorMode.MakeConnections && Rect.Contains(e.mousePosition))
					{
						OnStartConnection?.Invoke(this, e);
					}
					else if (Rect.Contains(e.mousePosition) && e.button == 0)
					{
						isDragged = true;
					}

					break;

				case EventType.MouseUp:
					if (Rect.Contains(e.mousePosition) && e.button == 0 && Mode == EditorMode.MakeConnections)
					{
						OnEndConnection?.Invoke(this, e);
					}

					if (e.button == 0)
					{
						isDragged = false;
					}

					break;

				case EventType.MouseDrag:
					if (e.button == 0)
					{
						switch (Mode)
						{
							case EditorMode.Drag:
								if (isDragged)
								{
									Drag(e.delta);
									e.Use();
								}

								break;
						}
					}
					break;

			}


			return false;
		}

		private void ProcessContextMenu()
		{
			var genericMenu = new GenericMenu();
			genericMenu.AddItem(new GUIContent("Delete room"), false, OnClickDelete);
			genericMenu.ShowAsContext();
		}

		private void OnClickDelete()
		{
			OnDelete?.Invoke(this);
		}

		public void Draw()
		{
			GUI.Box(Rect, Data.Name, style);
		}

		public void Drag(Vector2 delta)
		{
			Rect.position += delta;
			Data.Position += delta;
		}
	}
}