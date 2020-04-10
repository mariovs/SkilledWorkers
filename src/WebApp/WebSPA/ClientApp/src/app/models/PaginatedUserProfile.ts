import { UserProfile } from "./UserProfile";

export class PaginatedUserProfiles {
    PageIndex: number;

    PageSize: number;

    Count: number;

    Data: UserProfile[];
}