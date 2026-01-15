#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
184,
195,
196,
197,
198,
199,
200,
201,
202,
203,
206,
207,
318,
319,
320,
349,
350,
351,
378,
379,
380,
497,
498,
499,
502,
538,
539,
540,
541,
542,
546,
548,
550,
552,
558,
566,
567,
568,
569,
570,
571,
572,
573,
574,
665,
674,
675,
744,
750,
753,
755,
760,
761,
763,
764,
768,
769,
771,
772,
775,
776,
777,
780,
782,
785,
787,
789,
798,
865,
867,
869,
879,
880,
881,
883,
889,
890,
891,
892,
893,
901,
902,
903,
907,
908,
910,
912,
1118,
1282,
1283,
7565,
7566,
7568,
7569,
7570,
7571,
7572,
7574,
7575,
7576,
7594,
7596,
7603,
7605,
7607,
7609,
7612,
7662,
7663,
7665,
7666,
7667,
7668,
7669,
7671,
7673,
8801,
8805,
8807,
8808,
8809,
8810,
9226,
9227,
9228,
9229,
9247,
9248,
9249,
9251,
9301,
9371,
9374,
9382,
9383,
9384,
9385,
9386,
9680,
9685,
9686,
9714,
9749,
9756,
9763,
9774,
9778,
9801,
9879,
9881,
9890,
9892,
9893,
9900,
9915,
9935,
9936,
9944,
9946,
9953,
9954,
9957,
9959,
9964,
9970,
9971,
9978,
9980,
9992,
9995,
9996,
9997,
10008,
10018,
10024,
10025,
10026,
10028,
10029,
10046,
10048,
10063,
10081,
10108,
10138,
10139,
10681,
10765,
10766,
10950,
10951,
10955,
10956,
10957,
10962,
11013,
11427,
11428,
11632,
11637,
11647,
12555,
12576,
12578,
12580,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal (int);
int ves_icall_System_Array_IsValueOfElementTypeInternal (int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy (int,int,int,int,int);
int ves_icall_System_Array_GetLengthInternal_raw (int,int,int);
int ves_icall_System_Array_GetLowerBoundInternal_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
void ves_icall_System_Array_GetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
void ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
int ves_icall_System_Enum_InternalGetCorElementType (int);
void ves_icall_System_Enum_InternalGetUnderlyingType_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
int ves_icall_System_GC_GetMaxGeneration ();
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
int64_t ves_icall_System_GC_GetTotalAllocatedBytes_raw (int,int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_ModF (double,int);
int ves_icall_RuntimeMethodHandle_GetFunctionPointer_raw (int,int);
void ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw (int,int,int);
void ves_icall_RuntimeMethodHandle_ReboxToNullable_raw (int,int,int,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
void ves_icall_RuntimeType_make_array_type_raw (int,int,int,int);
void ves_icall_RuntimeType_make_byref_type_raw (int,int,int);
void ves_icall_RuntimeType_make_pointer_type_raw (int,int,int);
void ves_icall_RuntimeType_MakeGenericType_raw (int,int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
int ves_icall_System_RuntimeType_CreateInstanceInternal_raw (int,int);
void ves_icall_RuntimeType_GetDeclaringMethod_raw (int,int,int);
void ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetGenericArgumentsInternal_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition (int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetInterfaces_raw (int,int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringType_raw (int,int,int);
void ves_icall_RuntimeType_GetName_raw (int,int,int);
void ves_icall_RuntimeType_GetNamespace_raw (int,int,int);
int ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes (int);
int ves_icall_RuntimeTypeHandle_GetMetadataToken_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType (int);
int ves_icall_RuntimeTypeHandle_HasInstantiation (int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetModule_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition (int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
void ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
int64_t ves_icall_System_Threading_Monitor_Monitor_get_lock_contention_count ();
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
void ves_icall_System_Runtime_InteropServices_Marshal_GetDelegateForFunctionPointerInternal_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw (int,int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalBox_raw (int,int,int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
int ves_icall_System_Reflection_LoaderAllocatorScout_Destroy (int);
void ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
void ves_icall_InvokeClassConstructor_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int,int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
int ves_icall_System_Diagnostics_Debugger_IsAttached_internal ();
int ves_icall_System_Diagnostics_StackFrame_GetFrameInfo (int,int,int,int,int,int,int,int);
void ves_icall_System_Diagnostics_StackTrace_GetTrace (int,int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 184,
ves_icall_System_Array_InternalCreate,
// token 195,
ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal,
// token 196,
ves_icall_System_Array_IsValueOfElementTypeInternal,
// token 197,
ves_icall_System_Array_CanChangePrimitive,
// token 198,
ves_icall_System_Array_FastCopy,
// token 199,
ves_icall_System_Array_GetLengthInternal_raw,
// token 200,
ves_icall_System_Array_GetLowerBoundInternal_raw,
// token 201,
ves_icall_System_Array_GetGenericValue_icall,
// token 202,
ves_icall_System_Array_GetValueImpl_raw,
// token 203,
ves_icall_System_Array_SetGenericValue_icall,
// token 206,
ves_icall_System_Array_SetValueImpl_raw,
// token 207,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 318,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 319,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 320,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 349,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 350,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 351,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 378,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 379,
ves_icall_System_Enum_InternalGetCorElementType,
// token 380,
ves_icall_System_Enum_InternalGetUnderlyingType_raw,
// token 497,
ves_icall_System_Environment_get_ProcessorCount,
// token 498,
ves_icall_System_Environment_get_TickCount,
// token 499,
ves_icall_System_Environment_get_TickCount64,
// token 502,
ves_icall_System_Environment_FailFast_raw,
// token 538,
ves_icall_System_GC_GetCollectionCount,
// token 539,
ves_icall_System_GC_GetMaxGeneration,
// token 540,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 541,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 542,
ves_icall_System_GC_GetTotalAllocatedBytes_raw,
// token 546,
ves_icall_System_GC_SuppressFinalize_raw,
// token 548,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 550,
ves_icall_System_GC_GetGCMemoryInfo,
// token 552,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 558,
ves_icall_System_Object_MemberwiseClone_raw,
// token 566,
ves_icall_System_Math_Ceiling,
// token 567,
ves_icall_System_Math_Cos,
// token 568,
ves_icall_System_Math_Floor,
// token 569,
ves_icall_System_Math_Log,
// token 570,
ves_icall_System_Math_Pow,
// token 571,
ves_icall_System_Math_Sin,
// token 572,
ves_icall_System_Math_Sqrt,
// token 573,
ves_icall_System_Math_Tan,
// token 574,
ves_icall_System_Math_ModF,
// token 665,
ves_icall_RuntimeMethodHandle_GetFunctionPointer_raw,
// token 674,
ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw,
// token 675,
ves_icall_RuntimeMethodHandle_ReboxToNullable_raw,
// token 744,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 750,
ves_icall_RuntimeType_make_array_type_raw,
// token 753,
ves_icall_RuntimeType_make_byref_type_raw,
// token 755,
ves_icall_RuntimeType_make_pointer_type_raw,
// token 760,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 761,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 763,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 764,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 768,
ves_icall_System_RuntimeType_CreateInstanceInternal_raw,
// token 769,
ves_icall_RuntimeType_GetDeclaringMethod_raw,
// token 771,
ves_icall_System_RuntimeType_getFullName_raw,
// token 772,
ves_icall_RuntimeType_GetGenericArgumentsInternal_raw,
// token 775,
ves_icall_RuntimeType_GetGenericParameterPosition,
// token 776,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 777,
ves_icall_RuntimeType_GetFields_native_raw,
// token 780,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 782,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 785,
ves_icall_RuntimeType_GetDeclaringType_raw,
// token 787,
ves_icall_RuntimeType_GetName_raw,
// token 789,
ves_icall_RuntimeType_GetNamespace_raw,
// token 798,
ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw,
// token 865,
ves_icall_RuntimeTypeHandle_GetAttributes,
// token 867,
ves_icall_RuntimeTypeHandle_GetMetadataToken_raw,
// token 869,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 879,
ves_icall_RuntimeTypeHandle_GetCorElementType,
// token 880,
ves_icall_RuntimeTypeHandle_HasInstantiation,
// token 881,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 883,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 889,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 890,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 891,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 892,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 893,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 901,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 902,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition,
// token 903,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 907,
ves_icall_RuntimeTypeHandle_is_subclass_of_raw,
// token 908,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 910,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 912,
ves_icall_System_String_FastAllocateString_raw,
// token 1118,
ves_icall_System_Type_internal_from_handle_raw,
// token 1282,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1283,
ves_icall_System_ValueType_Equals_raw,
// token 7565,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 7566,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 7568,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 7569,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 7570,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 7571,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 7572,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 7574,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 7575,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 7576,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 7594,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 7596,
mono_monitor_exit_icall_raw,
// token 7603,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 7605,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 7607,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 7609,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 7612,
ves_icall_System_Threading_Monitor_Monitor_get_lock_contention_count,
// token 7662,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 7663,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 7665,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 7666,
ves_icall_System_Threading_Thread_GetState_raw,
// token 7667,
ves_icall_System_Threading_Thread_SetState_raw,
// token 7668,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 7669,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 7671,
ves_icall_System_Threading_Thread_YieldInternal,
// token 7673,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 8801,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 8805,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 8807,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 8808,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 8809,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 8810,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 9226,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 9227,
ves_icall_System_GCHandle_InternalFree_raw,
// token 9228,
ves_icall_System_GCHandle_InternalGet_raw,
// token 9229,
ves_icall_System_GCHandle_InternalSet_raw,
// token 9247,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 9248,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 9249,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 9251,
ves_icall_System_Runtime_InteropServices_Marshal_GetDelegateForFunctionPointerInternal_raw,
// token 9301,
ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw,
// token 9371,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw,
// token 9374,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 9382,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 9383,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 9384,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw,
// token 9385,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 9386,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalBox_raw,
// token 9680,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 9685,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 9686,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 9714,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 9749,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 9756,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 9763,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 9774,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 9778,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 9801,
ves_icall_System_Reflection_LoaderAllocatorScout_Destroy,
// token 9879,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 9881,
ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw,
// token 9890,
ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw,
// token 9892,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 9893,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 9900,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 9915,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 9935,
ves_icall_reflection_get_token_raw,
// token 9936,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 9944,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 9946,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 9953,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 9954,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 9957,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 9959,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 9964,
ves_icall_reflection_get_token_raw,
// token 9970,
ves_icall_get_method_info_raw,
// token 9971,
ves_icall_get_method_attributes,
// token 9978,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 9980,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 9992,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 9995,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 9996,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 9997,
ves_icall_reflection_get_token_raw,
// token 10008,
ves_icall_InternalInvoke_raw,
// token 10018,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 10024,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 10025,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 10026,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 10028,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 10029,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 10046,
ves_icall_InvokeClassConstructor_raw,
// token 10048,
ves_icall_InternalInvoke_raw,
// token 10063,
ves_icall_reflection_get_token_raw,
// token 10081,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 10108,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 10138,
ves_icall_reflection_get_token_raw,
// token 10139,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 10681,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 10765,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 10766,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 10950,
ves_icall_ModuleBuilder_basic_init_raw,
// token 10951,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 10955,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 10956,
ves_icall_ModuleBuilder_getToken_raw,
// token 10957,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 10962,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 11013,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 11427,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 11428,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 11632,
ves_icall_System_Diagnostics_Debugger_IsAttached_internal,
// token 11637,
ves_icall_System_Diagnostics_StackFrame_GetFrameInfo,
// token 11647,
ves_icall_System_Diagnostics_StackTrace_GetTrace,
// token 12555,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 12576,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 12578,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 12580,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_flags [] = {
0,
0,
0,
0,
0,
4,
4,
0,
4,
0,
4,
4,
0,
0,
0,
4,
4,
4,
4,
0,
4,
0,
0,
0,
4,
0,
0,
4,
4,
4,
4,
4,
0,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
0,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
};
