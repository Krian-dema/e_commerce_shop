<script setup>
import { watch, onUnmounted } from 'vue';
import { useWindowScroll } from '@vueuse/core';
import SlButton from '@/components/common/SlButton.vue';

const { x: scrollX, y: scrollY } = useWindowScroll();

const props = defineProps({
  showModal: {
    type: Boolean,
    required: true,
  },
  halt: {
    type: Boolean,
    required: false,
  },
  cancelLabel: {
    type: String,
    default: 'Cancel',
  },
  saveLabel: {
    type: String,
    default: 'Save',
  },
  styleClass: String,
});

const emit = defineEmits({
  handleCancel: null,
  handleSave: null,
});

watch(props, () => {
  if (props.showModal) {
    document.documentElement.style.overflow = 'hidden';
  }
});

const handleCancel = () => {
  document.documentElement.style.overflow = 'auto';
  emit('handleCancel', true);
};

const handleSave = () => {
  emit('handleSave', true);
};

onUnmounted(() => {
  document.documentElement.style.overflow = 'auto';
});
</script>

<template>
  <Teleport to="body">
    <Transition name="modal">
      <div
        v-if="showModal"
        class="absolute z-50 overflow-hidden bg-black bg-opacity-60 w-full h-full"
        :style="{ top: `${scrollY}px`, left: `${scrollX}px` }"
      >
        <div class="flex justify-center items-center w-full h-full">
          <div
            class="relative bg-white rounded-3xl shadow p-5 w-11/12 md:w-10/12 lg:w-3/5 xl:w-2/5"
            :class="styleClass"
          >
            <slot name="header"> </slot>
            <slot></slot>
            <slot name="footer">
              <div class="flex flex-row gap-5 justify-end pt-6">
                <SlButton
                  :disabled="halt"
                  :label="cancelLabel"
                  styleClass="btn-outlined px-3.5 py-2.5"
                  @handleClick="handleCancel"
                />
                <SlButton
                  :label="saveLabel"
                  :loading="halt"
                  class="px-3.5 py-2.5"
                  @handleClick="handleSave"
                />
              </div>
            </slot>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.modal-enter-active {
  transition: opacity 0.2s ease-out;
}
.modal-leave-active {
  transition: opacity 0.15s ease-out;
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}
</style>
