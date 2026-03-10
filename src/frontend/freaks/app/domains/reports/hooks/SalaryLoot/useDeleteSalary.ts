"use client";

import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { SalaryService } from "../../reports.service";

export function useDeleteSalary() {
  return useProtectedSWRMutation<void, number>(
    "/salaries/delete",
    (token, id) => SalaryService.deleteSalary(token, id),
  );
}
