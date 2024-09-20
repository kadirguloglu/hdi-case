import { axiosInstance } from "../client";
import { AuthenticationRequest } from "../../types/dto/authenticationDto/authenticationRequest";
import { AuthenticationResponse } from "../../types/dto/authenticationDto/authenticationResponse";
import { Result } from "../../types/abstract/result";

export const login = async (model: AuthenticationRequest) => {
  return await axiosInstance.post<Result<AuthenticationResponse>>(
    `/api/api/v1/Authentication/Login`,
    model
  );
};
