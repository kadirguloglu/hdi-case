// eslint-disable-next-line @typescript-eslint/no-explicit-any
export interface Result<T, K = any> {
  data: T | null;
  isAccessible?: boolean;
  data2?: K | null;
  isAccessible2?: boolean;
  isSuccessfull: boolean;
  message?: string | null;
}
