import { Skill } from "./Skill";

export class UserProfile {
    skills: Skill[];
    userId: string;
    firstName: string;
    lastName: string;
    streetName: string;
    city: string;
    country: string;
    state?: any;
    zipCode: string;
    lat: number;
    long: number;
}

