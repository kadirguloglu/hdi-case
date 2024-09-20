export interface AuthenticationResponse {
  bearerToken: string | null;
  userId: string | null;
  email: string | null;
}
