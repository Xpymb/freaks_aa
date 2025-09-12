export class AppError extends Error {
  code?: string;
  details?: object;
  status?: number;
  originalError?: unknown;
  constructor(
    message: string,
    code?: string,
    details?: object,
    status?: number,
    originalError?: unknown
  ) {
    super(message);
    this.name = "AppError";
    this.code = code || "UNKNOWN_ERROR";
    this.details = details;
    this.status = status;
    this.originalError = originalError;

    if (Error.captureStackTrace) {
      Error.captureStackTrace(this, AppError);
    }
  }
}
