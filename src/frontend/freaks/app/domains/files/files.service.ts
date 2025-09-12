import { authorizedApi } from "@/shared/api/authorizedApi";
import { IFileUploadResponse } from "./types";
import { AxiosProgressEvent } from "axios";

type UploadFileQuery = {
  raidId: number;
  fileType: number;
  file: File;
};

export const FileService = {
  postFile: (
    token: string,
    data: UploadFileQuery,
    onProgress?: (percent: number) => void
  ) => {
    const formData = new FormData();
    formData.append("file", data.file);

    return authorizedApi(token, "file").post<IFileUploadResponse>(
      `/raids/${data.raidId}/upload?fileType=${data.fileType}`,
      formData,
      {
        onUploadProgress: (event: AxiosProgressEvent) => {
          if (onProgress && event.total) {
            const percent = Math.round((event.loaded * 100) / event.total);
            onProgress(percent);
          }
        },
      }
    );
  },
};
