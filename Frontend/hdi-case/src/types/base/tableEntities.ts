export interface TableEntities<T> {
  id: string | null;
  createdDate: string;
  lastUpdatedDate: string;
  collectionName: string;
  data: T;
}
