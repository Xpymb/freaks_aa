export interface PaginatedList<T = unknown> {
  items: T[];
  take: number | null;
  skip: number | null;
  totalCount: number;
}
