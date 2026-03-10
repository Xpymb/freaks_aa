"use client";

import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { SalaryService } from "../../reports.service";
import { CreateSalaryRequest, SalaryDto } from "../../types";

export function useCreateSalary() {
  return useProtectedSWRMutation<SalaryDto, CreateSalaryRequest>(
    "/salaries",
    (token, data) => SalaryService.createSalary(token, data),
  );
}
