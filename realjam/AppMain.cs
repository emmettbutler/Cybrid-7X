using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace realjam
{
	public class AppMain
	{
		
		public static void Main (string[] args)
		{
      Director.Initialize();
      Director.Instance.GL.Context.SetClearColor(new Vector4(36.0f/255.0f, 234.0f/255.0f, 143.0f/255.0f, 1.0f));

      var scene = new Scene();
      scene.Camera.SetViewFromViewport();

      var collider = new Collider();

      var player = new Player(new Vector2(40,10));
      scene.AddChild(player.sprite);
      collider.add(player);

      SpawnManager spawnmngr = new SpawnManager(scene,collider);

      Director.Instance.RunWithScene(scene, true);

      Scheduler.Instance.Schedule(scene,spawnmngr.FrameUpdate,0.0f,false);
      Scheduler.Instance.Schedule(scene,player.Tick,0.0f,false);

			while (!Input2.GamePad0.Cross.Press) {
        SystemEvents.CheckEvents();
        Director.Instance.Update();
        Director.Instance.Render();

        collider.CollideWalls(scene);
        collider.Collide();

        Director.Instance.GL.Context.SwapBuffers();
        Director.Instance.PostSwap();
			}

      Director.Terminate();
		}
	}
}
