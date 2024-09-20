import { TableEntities } from "../../base/tableEntities";
import { Enum_AggrementStatus } from "../../enums/Enum_AggrementStatus";

export interface Aggrement extends TableEntities<Aggrement> {
  startDate: string;
  endDate: string;
  riskRate: number;
  riskAmount: number;
  companyId: number;
  status: Enum_AggrementStatus;
  rejectDescription: string | null;
}
