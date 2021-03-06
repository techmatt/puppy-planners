﻿using game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class PuppySkill
    {
        public PuppySkill()
        {

        }
        public PuppySkill(SkillInfo info, Random random)
        {
            name = info.name;
            learningRate = info.sampleLearningRate(random);
            totalTraining = 0.0;
        }
        public string name;
        public double learningRate; // value between 0 and infinity; direct scale factor on totalTraining
        public double totalTraining; // training units are in modified seconds and does already includes learningRate (ex. without research, this is 1 training unit/s)

    }

    public class HappinessModifier
    {
        public HappinessModifier()
        {

        }
        public HappinessModifier(string _id, string _description, double _strength)
        {
            id = _id;
            description = _description;
            fullStrength = _strength;
        }
        public bool isPermanent()
        {
            return (fadeTimeTotal > 0.0);
        }
        public double currentStrength()
        {
            // TODO: implement fading happiness modifiers
            return fullStrength;
        }

        public string id;
        public string description;
        public double fadeTimeTotal;
        public double fadeTimeLeft;
        public double fullStrength;
    }

    public class Puppy
    {
        public string name;
        public string initials;

		public CombatStats combat = new CombatStats ();

		// the name of the role owned by this puppy (designated by Culture)
		// this should always be one of the Roles in Constants.playerRoles
		public string assignedPlayer = "unassigned";

		//task is the task that the puppy is assigned for each Role
		public Dictionary<string,string> tasks=new Dictionary<string,string>();
		public string task; // should always be tasks[assignedPlayer]

        // task is the name of the skill being used
        // possible tasks:
        // scout, military, work, construction
        //public string task = "none";

        // the name of the skill being learned (only relevant of the puppy is at a school-equivalent building)
        public string learningSkill = "none";

        // workLocation = Constants.invalidCoord if the puppy is not assigned a job. workLocation includes military duty.
        public Coord workLocation = Constants.invalidCoord;

        // homeLocation = Constants.invalidCoord if the puppy is homeless
        public Coord homeLocation = Constants.invalidCoord;

        // cultureLocation = park, statue, or other recreational site
        public Coord cultureLocation = Constants.invalidCoord;

        // religionLocation = location of the church they attend
        public Coord churchLocation = Constants.invalidCoord;


		// the current grid coordinates of the puppy
		public DoubleCoord currentLocation = Constants.invalidDoubleCoord;
		public bool currentlyMoving = false;
		public DoubleCoord destination = Constants.invalidDoubleCoord;
		public bool isEmployee = false;

        public double health = 1.0;
        public double corruption = 0.0;
        public double happiness;
		public double movementRate = 0.2; //grid squares/tick

        // see skills.csv for a list of skill names
        public Dictionary<string, PuppySkill> skills = new Dictionary<string, PuppySkill>();
        public List<string> attributes = new List<string>();
        public Dictionary<string, HappinessModifier> happinessMods = new Dictionary<string, HappinessModifier>();

        public double skillEffectiveness(string skillName)
        {
            return happiness * GameFunctions.skillTrainingToEffectiveness(skills[skillName].totalTraining);
        }

        public string describe()
        {
            var s = new StringBuilder();
            s.AppendLine("Initials = " + initials);
            s.AppendLine("Full name = " + name);
            s.AppendLine("Assigned to " + assignedPlayer);
			s.AppendLine("Task = " + tasks[assignedPlayer]);
            s.AppendLine("Skill being learned = " + learningSkill);
            s.AppendLine("Work location = " + workLocation.ToString());
            s.AppendLine("Home location = " + homeLocation.ToString());
            s.AppendLine("Culture location = " + cultureLocation.ToString());
            s.AppendLine("Church location = " + churchLocation.ToString());
            s.AppendLine("Health = " + health.ToString());
			s.AppendLine("Location = " + currentLocation.ToString ());
			s.AppendLine("Is moving = " + currentlyMoving.ToString ());
			s.AppendLine ("Destination = " + destination.ToString ());


            s.AppendLine();
            s.AppendLine("Happiness:");
            foreach(HappinessModifier m in happinessMods.Values)
                s.AppendLine("  " + m.description + " (+" + m.fullStrength.ToString("#.##") + ")");

            s.AppendLine();
            s.AppendLine("Skills:");
            foreach (PuppySkill m in skills.Values)
                s.AppendLine("  " + m.name + " (time=" + m.totalTraining + ", rate=" + m.learningRate + ")");

            s.AppendLine();
            s.AppendLine("Attributes:");
            foreach (PuppySkill m in skills.Values)
                s.AppendLine("  " + m);
            
            return s.ToString();
        }

        public Puppy()
        {

        }

        public Puppy(GameState state)
        {
			PuppyName puppyName = Database.Get.randomPuppyName(state);
            name = puppyName.fullName;
            initials = puppyName.initials;
            
			foreach(SkillInfo skill in Database.Get.puppySkills.Values)
            {
                skills[skill.name] = new PuppySkill(skill, state.random);
            }

			// initialize the roles
			foreach (string r in Constants.playerRoles.Keys)
				tasks.Add(r, "");

            //
            // TODO: add random puppy attributes
            //
        }

        void recordHappiness(string id, string description, double strength)
        {
            happinessMods[id] = new HappinessModifier(id, description, strength);
        }
        
		public void assignRole(string _assignedPlayer)
		{
			assignedPlayer = _assignedPlayer;
			task = tasks [assignedPlayer];
		}

		public void assignTask(string _assignedPlayer, string _task)
		{
			assignRole(_assignedPlayer);
			task = _task;
			tasks [assignedPlayer] = task;
			Console.WriteLine ("assigned task");
		}

		public void moveToCell(MapCell c)
		{
			workLocation = c.coord;
			destination = workLocation.toDouble ();
			isEmployee = false;
		}

        public void updateHappiness()
        {
            if (homeLocation.isValid())
                recordHappiness("home", "Has a home", 0.5);
            else
                recordHappiness("home", "Homeless!", 0.0);

            if (churchLocation.isValid())
                recordHappiness("church", "Attends a church", 0.1);
            else
                recordHappiness("church", "No church available", 0.0);

            if (cultureLocation.isValid())
                recordHappiness("church", "Has a <cultural site name>", 0.2);
            else
                recordHappiness("church", "No <cultural site name> available", 0.0);

            if (workLocation.isValid())
                recordHappiness("work", "Has a job", 0.0);
            else
                recordHappiness("work", "On vacation!", 0.2);

            happiness = 0.0;
            foreach(HappinessModifier h in happinessMods.Values)
            {
                happiness += h.currentStrength();
            }
            happiness = Util.bound(happiness, 0.0, 3.0);
        }
			
    }
}
