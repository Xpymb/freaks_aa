import { KeycloakProfile } from "next-auth/providers/keycloak";

export interface IKeycloakUser extends KeycloakProfile {
  game_nickname?: string;
  roles?: string[];
}
