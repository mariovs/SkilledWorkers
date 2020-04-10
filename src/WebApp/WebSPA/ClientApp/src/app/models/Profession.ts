import { SkillLevel } from './SkillLevel';

export class Profession {

    constructor(name, availableSkills){
        this.name =name;
        this.availableSkillLevels = availableSkills;
    }


    name: string;
    availableSkillLevels: SkillLevel[];
}

