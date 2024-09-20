import { axiosInstance } from "../client";
import { Result } from "../../types/abstract/result";
import { GetCurrentUserResponse } from "../../types/dto/authenticationDto/getCurrentUserResponse";

export const getCurrentUser = async () => {
  return await axiosInstance
    .get<Result<GetCurrentUserResponse>>(
      `/api/api/v1/authentication/GetCurrentUser`
    )
    .then((x) => x.data);
};
