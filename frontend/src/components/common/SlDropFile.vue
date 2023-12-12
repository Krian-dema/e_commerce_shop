<script setup>
import { ref, computed, onMounted } from 'vue';

import SlButton from '@/components/common/SlButton.vue';
import SlIconClose from '@/components/common/icons/SlIconClose.vue';
import { ARCHIVE_EXTENSIONS, IMAGE_EXTENSIONS } from '@/constants/fileExtensions';

const files = ref([]);
const isDragging = ref(false);
const additionalError = ref('');

const props = defineProps({
  dropFileId: {
    type: String,
    required: true,
  },
  label: String,
  extensions: {
    type: Array,
    default: () => [...ARCHIVE_EXTENSIONS, ...IMAGE_EXTENSIONS],
  },
  defaultFile: File,
  error: String,
});

const emit = defineEmits({ selectFile: null });

const hasError = computed(() => !!props.error || additionalError.value);
const errorMessage = computed(() => props.error ?? additionalError.value);
const acceptExtensions = computed(() => props.extensions.join(','));

const handleChange = (e) => {
  const filesReceived = e.target.files || e.dataTransfer.files;

  if (!filesReceived.length) return;
  else if (files.value.length) {
    files.value.splice(0, files.value.length);
  }

  const file = filesReceived[0];
  if (!props.extensions.some((extension) => file.name.includes(extension))) return;

  files.value.push(file);
  emit('selectFile', file);
};

const dragover = (e) => {
  e.preventDefault();
  isDragging.value = true;
};

const dragleave = (e) => {
  e.preventDefault();
  isDragging.value = true;
};

const drop = (e) => {
  e.preventDefault();
  handleChange(e);
  isDragging.value = false;
};

const remove = (file) => {
  const index = files.value.indexOf(file);
  files.value.splice(index, 1);
  emit('selectFile', null);
};

onMounted(() => {
  if (props.defaultFile?.name) {
    files.value.push(props.defaultFile);
  }
});
</script>

<template>
  <div class="flex flex-col">
    <label class="text-xs mb-2">{{ label }}</label>
    <div
      v-if="!files.length"
      class="flex justify-center items-center border border-dashed border-cyan-800 rounded-lg h-40 w-full"
      :class="{
        'bg-primary-200': isDragging,
        'sl-error': hasError,
      }"
      @dragover="dragover"
      @dragleave="dragleave"
      @drop="drop"
    >
      <input
        type="file"
        :id="dropFileId"
        :name="dropFileId"
        class="absolute opacity-0 overflow-hidden w-1 h-1"
        :accept="acceptExtensions"
        @change="handleChange"
      />

      <label :for="dropFileId" class="text-primary-800 hover:cursor-pointer">
        <div v-if="isDragging"><slot name="dragContent">Release to drop files here.</slot></div>
        <div v-else>
          <slot name="dropContent"
            >Drop files here or
            <span class="underline underline-offset-4 font-semibold">click here</span> to
            upload.</slot
          >
        </div>
      </label>
    </div>

    <div v-if="files.length" class="flex flex-col gap-4">
      <div
        v-for="file in files"
        :key="file?.name"
        class="inline-flex justify-start items-center gap-4 border rounded-lg px-4 py-2 max-w-max"
      >
        <slot name="preview" :file="file"> </slot>
        <SlButton styleClass="btn-link" @handleClick="() => remove(file)">
          <template #icon> <SlIconClose class="text-dark-500 w-5 h-5 stroke-1.5" /> </template>
        </SlButton>
      </div>
    </div>

    <div v-if="hasError" class="text-xs text-red-500 mt-1">
      {{ errorMessage }}
    </div>
  </div>
</template>

<style scoped>
.sl-error {
  @apply border-red-500 focus:text-black focus:border-red-500 focus:ring-red-500;
}
</style>
