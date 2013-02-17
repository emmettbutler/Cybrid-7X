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
      WLK_RIGHT = 0x0001,
      WLK_LEFT = 0x0002,
      WLK_UP = 0x0004,
      WLK_DOWN = 0x0008,
      WLK_NONE = 0x0000
    };

    private WalkDirs walkDirection;
    private Boolean walking = false;
    private Boolean carryplant = false;

    private Boolean walkup = false;
    private Boolean walkdown = false;
    private Boolean walkright = false;
    private Boolean walkleft = false;
    private Boolean walknone = false;

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
      if(Input2.GamePad0.Circle.Down && canGrab){
        if(instance is Cell){
          Cell c = (Cell)instance;
          //c.sprite.Position = new Vector2(c.sprite.Position.X+10, c.sprite.Position.Y+10);
          c.grabbed = true;
          grabbing = c;
          }
      } else {
        if(!(instance is Cell)) return;
        Cell c = (Cell)instance;
        //instance.sprite.Position += (instance.sprite.Position-sprite.Position)*.2f;
        Vector2 cellCenter = c.GetCenter();
        Vector2 playerCenter = this.GetCenter();
        Vector2 displacement = cellCenter - playerCenter;
        if(System.Math.Abs(displacement.X) > System.Math.Abs(displacement.Y)){
          if(playerCenter.X < cellCenter.X){
            sprite.Position = new Vector2(sprite.Position.X - runSpeed,sprite.Position.Y);
          } else if(playerCenter.X > cellCenter.X){
            sprite.Position = new Vector2(sprite.Position.X + runSpeed,sprite.Position.Y);
          }
        } else {
          if(playerCenter.Y < cellCenter.Y){
            sprite.Position = new Vector2(sprite.Position.X,sprite.Position.Y - runSpeed);
          } else if(playerCenter.Y > cellCenter.Y){
            sprite.Position = new Vector2(sprite.Position.X,sprite.Position.Y + runSpeed);
          }
        }
      }
    }

    public override void CollideFrom (GameEntity instance){
    }

    public override void Tick(float dt){
      base.Tick(dt);
      walking = false;
      Vector2 delta = Vector2.Zero;

      if(Input2.GamePad0.Right.Down){
        walking = true;
        walkright = true;
        if(walkDirection != WalkDirs.WLK_RIGHT){
          sprite.StopAllActions();
          sprite.RunAction(WalkRightAnimation);

        }
        walkDirection |= WalkDirs.WLK_RIGHT;
        delta = new Vector2(runSpeed,0);
      } else if(Input2.GamePad0.Left.Down){
        walking = true;
        walkleft = true;
        if(walkDirection != WalkDirs.WLK_LEFT){
          sprite.StopAllActions();
          sprite.RunAction(WalkLeftAnimation);

        }
        walkDirection |= WalkDirs.WLK_LEFT;
        delta = new Vector2(-runSpeed,0);
      }

      if(Input2.GamePad0.Up.Down){
        walking = true;
        walkup = true;
        if(walkDirection != WalkDirs.WLK_UP){
          sprite.StopAllActions();
          sprite.RunAction(WalkBackAnimation);

        }
        walkDirection |= WalkDirs.WLK_UP;
        delta += new Vector2(0,runSpeed);
      } else if(Input2.GamePad0.Down.Down){
        walking = true;
        walkdown = true;
        if(walkDirection != WalkDirs.WLK_DOWN){
          sprite.StopAllActions();
          sprite.RunAction(WalkFrontAnimation);

        }
        walkDirection |= WalkDirs.WLK_DOWN;
        delta += new Vector2(0,-runSpeed);
      }

      sprite.Position += delta;
      walknone = true;

      if(Input2.GamePad0.Square.Down){
        Support.SoundSystem.Instance.Play("WaterPlop_2.wav");
        waterClosestPlants();
      }

      if(!walking){
        sprite.StopAllActions();
        walkDirection = WalkDirs.WLK_NONE;
      }
      if(grabbing != null){
        carryplant = true;
        if(carryplant != false){
          Support.SoundSystem.Instance.Play("grass.wav");
        }
        carryplant = false;
        grabbing.sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y+70);
      }
      if(!Input2.GamePad0.Circle.Down && grabbing != null){
        grabbing.grabbed = false;
        grabbing = null;
      }
    }

    public override float GetRadius(){
       return (sprite.Quad.X.X > sprite.Quad.Y.Y ? sprite.Quad.Y.Y : sprite.Quad.X.X)/2;
    }

    public void waterClosestPlants(){
      var drops = Support.TiledSpriteFromFile("/Application/assets/Water_Object.png", 13, 1);

      if(walkdown == true){
        drops.Position = new Vector2(this.sprite.Position.X-20,this.sprite.Position.Y-50);
        walkdown = false;
      } else if(walkleft == true){
        drops.Position = new Vector2(this.sprite.Position.X-60,this.sprite.Position.Y-40);
        walkleft = false;
      } else if(walkright == true){
        drops.Position = new Vector2(this.sprite.Position.X+20,this.sprite.Position.Y-45);
        walkright = false;
      } else if(walkup == true){
        drops.Position = new Vector2(this.sprite.Position.X-30,this.sprite.Position.Y+50);
        walkup = false;
      }

      drops.VertexZ = 1;
      var WateringAnimation = new Support.AnimationAction(drops, 13, 1, 1.0f, looping: true);

      GameScene.Instance.AddChild(drops);

      var seq = new Sequence();
      drops.RunAction(WateringAnimation);
      seq.Add(new DelayTime(.8f));
      seq.Add(new CallFunc(() => {GameScene.Instance.RemoveChild(drops, true); }));
      GameScene.Instance.RunAction(seq);

      for(int i = 0; i < SpawnManager.Instance.cells.Count; i++){
        Vector2 displacement = sprite.Position - SpawnManager.Instance.cells[i].sprite.Position;
        if(displacement.Length() < 80){
          SpawnManager.Instance.cells[i].watered = true;
        }
      }
    }
  }
}

