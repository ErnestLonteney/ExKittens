using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploadingKittens
{
    abstract class Card
    {
        public byte Number { get; set; }
        public override string ToString()
        {
            return base.ToString().Replace("ExploadingKittens.", String.Empty);
        }
        public abstract void ResolveCardEffect();
        public Card(byte number)
        {
            Number = number;
        }
    }
    class ExKitten : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }

        public ExKitten(byte n)
           :base(n) 
        {

        }
    }
    class Defuse : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public Defuse(byte n)
           :base(n) 
        {

        }
    }
    class Attack : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public Attack(byte n)
           :base(n) 
        {

        }
    }
    class SeeTheFeuture : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public SeeTheFeuture(byte n)
           :base(n) 
        {

        }
    }
    class AlterTheFeauture : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public AlterTheFeauture(byte n)
           :base(n) 
        {

        }
    }
    class TargetAttack : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public TargetAttack(byte n)
           :base(n) 
        {

        }
    }
    class Blank : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public readonly BlankType _blank;
        public Blank(BlankType b, byte n)
            :base(n)
        {
            _blank = b;
        }
    }
    class Nope : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public Nope(byte n)
           :base(n) 
        {

        }
    }
    class Favor : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public Favor(byte n)
           :base(n) 
        {

        }
    }
    class Reverse : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public Reverse(byte n)
           :base(n) 
        {

        }
    }
    class Skip : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public Skip(byte n)
           :base(n) 
        {

        }
    }
    class Shufle : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public Shufle(byte n)
           :base(n) 
        {

        }
    }
    class ImpladingKitten : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public bool Flag { get; set; } = false;
        public ImpladingKitten(byte n)
           :base(n) 
        {

        }
    }
    class DrawFromTheBottom : Card
    {
        public static Action Resolve { get; set; } = null;
        public override void ResolveCardEffect()
        {
            Resolve.Invoke();
        }
        public DrawFromTheBottom(byte n)
           :base(n) 
        {

        }
    }
}
