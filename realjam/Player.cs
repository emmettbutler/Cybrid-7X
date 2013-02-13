using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class Player:GameEntity {

    public const int runSpeed = 7;
    public Cell grabbing = null;
    public Support.AnimationAction WalkFrontAnimation { get; set; }
    public Support.AnimationAction WalkBackAnimation { get; set; }
    public Support.AnimationAction WalkLeftAnimation { get; set; }
    public Support.AnimationAction WalkRightAnimation { get; set; }

    public Player(Vector2 pos) : base(pos){
      sprite = Support.TiledSpriteFromFile("/Application/assets/robot_sheet1.png", 9, 1);
      sprite.CenterSprite();

      WalkFrontAnimation = new Support.AnimationAction(sprite, 1, 2, 1.0f, looping: true);
      WalkBackAnimation = new Support.AnimationAction(sprite, 3, 4, 1.0f, looping: true);
      WalkLeftAnimation = new Support.AnimationAction(sprite, 5, 6, 1.0f, looping: true);
      WalkRightAnimation = new Support.AnimationAction(sprite, 7, 9, 1.0f, looping: true);
    }

    public override void CollideTo (GameEntity instance){
      Boolean canGrab = (instance == grabbing || grabbing == null);
      if(instance is Cell && Input2.GamePad0.Circle.Down && canGrab){
        Cell c = (Cell)instance;
        c.sprite.Position = sprite.Position;
        c.grabbed = true;
        grabbing = c;
      } else {
        instance.sprite.Position += (instance.sprite.Position-sprite.Position)*.2f;
      }
    }

    public override void CollideFrom (GameEntity instance){
    }

    public override void Tick(float dt){
      base.Tick(dt);
      //Console.WriteLine(ttime);
      Vector2 delta = Vector2.Zero;

      if(Input2.GamePad0.Right.Down){
        delta = new Vector2(runSpeed,0);
        sprite.RunAction(WalkRightAnimation);
      } else if(Input2.GamePad0.Left.Down){
        delta = new Vector2(-runSpeed,0);
        sprite.RunAction(WalkLeftAnimation);
      }

      if(Input2.GamePad0.Up.Down){
        delta += new Vector2(0,runSpeed);
        sprite.RunAction(WalkFrontAnimation);
      } else if(Input2.GamePad0.Down.Down){
        delta += new Vector2(0,-runSpeed);
        sprite.RunAction(WalkBackAnimation);
      }
      Position += delta;
      sprite.Position += delta;

      if(!Input2.GamePad0.Circle.Down && grabbing != null){
        grabbing.grabbed = false;
        grabbing = null;
      }
    }
  }
}

