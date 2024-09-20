import { TableEntities } from "../base/tableEntities";

export interface Company extends TableEntities<Company> {
  name: string;
  phones: string[];
  emails: string[];
  apiKey: string;
  apiIsActive: boolean;
  apiPerMinuteMaximumRequestCount: number;
  aggrementResultWebhookUrl: string | null;
}
