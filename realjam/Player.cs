using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class Player:GameEntity {

    public const int runSpeed = 5;
    public Cell grabbing = null;

    public Player(Vector2 pos) : base(pos){
      texture = new TextureInfo( new Texture2D("/Application/assets/robot.png",false));
      TextureInfo = texture;
      Quad.S = texture.TextureSizef/2;
    }

    public override void CollideTo (GameEntity instance){
      Boolean canGrab = (instance == grabbing || grabbing == null);
      if(instance is Cell && Input2.GamePad0.Circle.Down && canGrab){
        Cell c = (Cell)instance;
        c.Position = Position;
        c.grabbed = true;
        grabbing = c;
      } else {
        instance.Position += (instance.Position-Position)*.2f;
      }
    }

    public override void CollideFrom (GameEntity instance){
    }

    public override void Tick(float dt){
      base.Tick(dt);
      Console.WriteLine(ttime);
      Vector2 delta = Vector2.Zero;

      if(Input2.GamePad0.Right.Down){
        delta = new Vector2(runSpeed,0);
      } else if(Input2.GamePad0.Left.Down){
        delta = new Vector2(-runSpeed,0);
      }

      if(Input2.GamePad0.Up.Down){
        delta += new Vector2(0,runSpeed);
      } else if(Input2.GamePad0.Down.Down){
        delta += new Vector2(0,-runSpeed);
      }
      Position += delta;

      if(!Input2.GamePad0.Circle.Down && grabbing != null){
        grabbing.grabbed = false;
        grabbing = null;
      }
    }
  }
}

