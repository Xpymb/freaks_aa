"use client";

import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { SalaryService } from "../../reports.service";
import { SalaryDto, UpdateSalaryRequest } from "../../types";

type UpdateArgs = {
  id: number;
  data: UpdateSalaryRequest;
};

export function useUpdateSalary() {
  return useProtectedSWRMutation<SalaryDto, UpdateArgs>(
    "/salaries/update",
    (token, { id, data }) => SalaryService.mutateSalary(token, id, data),
  );
}
