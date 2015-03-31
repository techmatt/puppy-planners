using game;
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
        public string type;
        public double fadeTimeTotal; // fadeTimeTotal = 0 means the effect is permanent
        public double fadeTimeLeft;
        public double fullStrength;
    }

    public class Puppy
    {
        public string name;
        public string initials;

        // task is the name of the skill being used
        public string task = "none";

        // the name of the skill being learned (only relevant of the puppy is at a school-equivalent building)
        public string learningSkill = "none";

        // workLocation = (-1, -1) if the puppy is not assigned a task
        public Coord workLocation = new Coord(-1, -1);

        // homeLocation = (-1, -1) if the puppy is homeless
        public Coord homeLocation = new Coord(-1, -1);

        // the name of the role owned by this puppy (designated by Culture)
        public string assignedPlayer = "none";

        public double health = 1.0;
        public double corruption = 0.0;

        public Dictionary<string, PuppySkill> skills = new Dictionary<string, PuppySkill>();
        public List<string> attributes = new List<string>();
        public List<HappinessModifier> modifiers = new List<HappinessModifier>();

        public Puppy()
        {

        }

        public Puppy(GameState state)
        {
            PuppyName puppyName = state.database.randomPuppyName(state);
            name = puppyName.fullName;
            initials = puppyName.initials;
            
            foreach(SkillInfo skill in state.database.puppySkills.Values)
            {
                skills[skill.name] = new PuppySkill(skill, state.random);
            }

            //
            // TODO: add random puppy attributes
            //
        }
    }
}
