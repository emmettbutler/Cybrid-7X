using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;

namespace realjam {
  public class Player:GameEntity {

    public const int runSpeed = 3;
    public Cell grabbing = null;
    public Support.AnimationAction WalkFrontAnimation { get; set; }
    public Support.AnimationAction WalkBackAnimation { get; set; }
    public Support.AnimationAction WalkLeftAnimation { get; set; }
    public Support.AnimationAction WalkRightAnimation { get; set; }

    private enum WalkDirs{
      WLK_RIGHT, WLK_LEFT, WLK_UP, WLK_DOWN, WLK_NONE
    };

    private WalkDirs walkDirection;
    private Boolean walking = false;

    public Player(Vector2 pos) : base(pos){
      sprite = Support.TiledSpriteFromFile("/Application/assets/robot_sheet2.png", 9, 4);
      sprite.CenterSprite();

      WalkLeftAnimation = new Support.AnimationAction(sprite, 2, 9, 1.0f, looping: true);
      WalkRightAnimation = new Support.AnimationAction(sprite, 20, 27, 1.0f, looping: true);
      WalkBackAnimation = new Support.AnimationAction(sprite, 11, 18, 1.0f, looping: true);
      WalkFrontAnimation = new Support.AnimationAction(sprite, 29, 36, 1.0f, looping: true);

      walkDirection = WalkDirs.WLK_NONE;
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
      walking = false;
      Vector2 delta = Vector2.Zero;

      if(Input2.GamePad0.Right.Down){
        walking = true;
        if(walkDirection != WalkDirs.WLK_RIGHT){
          sprite.StopAllActions();
          sprite.RunAction(WalkRightAnimation);
        }
        walkDirection = WalkDirs.WLK_RIGHT;
        delta = new Vector2(runSpeed,0);
      } else if(Input2.GamePad0.Left.Down){
        walking = true;
        if(walkDirection != WalkDirs.WLK_LEFT){
          sprite.StopAllActions();
          sprite.RunAction(WalkLeftAnimation);
        }
        walkDirection = WalkDirs.WLK_LEFT;
        delta = new Vector2(-runSpeed,0);
      }

      if(Input2.GamePad0.Up.Down){
        walking = true;
        if(walkDirection != WalkDirs.WLK_UP){
          sprite.StopAllActions();
          sprite.RunAction(WalkBackAnimation);
        }
        walkDirection = WalkDirs.WLK_UP;
        delta += new Vector2(0,runSpeed);
      } else if(Input2.GamePad0.Down.Down){
        walking = true;
        if(walkDirection != WalkDirs.WLK_DOWN){
          sprite.StopAllActions();
          sprite.RunAction(WalkFrontAnimation);
        }
        walkDirection = WalkDirs.WLK_DOWN;
        delta += new Vector2(0,-runSpeed);
      }
      sprite.Position += delta;

      if(!walking){
        sprite.StopAllActions();
        walkDirection = WalkDirs.WLK_NONE;
      }

      if(!Input2.GamePad0.Circle.Down && grabbing != null){
        grabbing.grabbed = false;
        grabbing = null;
      }
    }
  }
}

