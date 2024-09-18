export interface TableEntities<T> {
  _Id: string | null;
  createdDate: string;
  lastUpdatedDate: string;
  collectionName: string;
  data: T;
}
