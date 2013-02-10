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
      Director.Instance.GL.Context.SetClearColor(Colors.Cyan);

      var scene = new Scene();
      scene.Camera.SetViewFromViewport();

      var sprite = new Cell();

      sprite.CenterSprite();

      sprite.Position = scene.Camera.CalcBounds().Center;

      scene.AddChild(sprite);

      Director.Instance.RunWithScene(scene, true);

      Scheduler.Instance.Schedule(scene,sprite.Tick,0.0f,false);

			while (!Input2.GamePad0.Cross.Press) {
        SystemEvents.CheckEvents();
        Director.Instance.Update();
        Director.Instance.Render();

        Director.Instance.GL.Context.SwapBuffers();
        Director.Instance.PostSwap();
			}

      Director.Terminate();
		}
	}
}
